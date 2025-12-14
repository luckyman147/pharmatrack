using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.enums;
using PharmaTrack.Domain.Models.depot;
using PharmaTrack.Domain.Models.Shared;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.commande.UpdateCommandeStatus;

public class UpdateCommandeStatusCommandHandler(
    ICommandeRepository commandeRepository,
    IDepotRepository depotRepository,
    IStockTransactionHistoryRepository historyRepository)
    : ICommandHandler<UpdateCommandeStatusCommand, UpdateCommandeStatusResult>
{
    public async Task<UpdateCommandeStatusResult> Handle(UpdateCommandeStatusCommand request, CancellationToken cancellationToken)
    {
        var commande = await commandeRepository.GetByIdAsync(request.Id);
        if (commande == null)
            throw new Exception($"Commande {request.Id} not found");

        var oldStatus = commande.Status;
        if (oldStatus == request.NewStatus)
            return new UpdateCommandeStatusResult(true);

        // Update status
        commande.Status = request.NewStatus;
        await commandeRepository.UpdateAsync(commande);

        // Handle Stock Updates
        // 1. Success (Valide) -> Deduct Stock
        if (request.NewStatus == StatusCommande.Valide && oldStatus != StatusCommande.Valide)
        {
            await AdjustStock(commande, deduct: true, TransactionReason.CommandValidated);
        }
        // 2. Cancelled (Annulée) -> Return Stock (if it was previously Valid/Deducted)
        else if ((request.NewStatus == StatusCommande.Annulee || request.NewStatus == StatusCommande.EnRetour) && oldStatus == StatusCommande.Valide)
        {
            var reason = request.NewStatus == StatusCommande.Annulee ? TransactionReason.CommandCancelled : TransactionReason.StockReturn;
            await AdjustStock(commande, deduct: false, reason); 
        }

        return new UpdateCommandeStatusResult(true);
    }

    private async Task AdjustStock(PharmaTrack.Domain.Models.Commande.Commande commande, bool deduct, TransactionReason                                                                                         reason)
    {
        // Group by depot
        var allocations = commande.Lines
            .SelectMany(l => l.allocationsDepots.Select(a => new { l.ProduitId, DepotId = a.DepotInfo.Id, a.QuantityAllocated }))
            .GroupBy(x => x.DepotId);

        foreach (var group in allocations)
        {
            var depotId = group.Key;
            var depot = await depotRepository.GetByIdAsync(depotId);
            if (depot == null) continue;

            foreach (var item in group)
            {
                var productStock = depot.ProduitStock.FirstOrDefault(p => p.ProduitId == item.ProduitId);
                if (productStock != null)
                {
                    int oldQty = productStock.Quantity;
                    int adjustment = item.QuantityAllocated;

                    if (deduct)
                        productStock.Quantity -= adjustment;
                    else
                        productStock.Quantity += adjustment;

                    // "Trigger" - Log History
                    var history = new StockTransactionHistory
                    {
                        DepotId = depotId,
                        ProduitId = item.ProduitId,
                        DepotName = depot.Name,
                        ProduitName = productStock.ProduitNom,
                        CommandeId = commande.id,
                        OldQuantity = oldQty,
                        NewQuantity = productStock.Quantity,
                        Reason = reason,
                        TransactionDate = DateTime.UtcNow
                    };
                    await historyRepository.AddAsync(history);
                }
            }
            await depotRepository.UpdateAsync(depot);
        }
    }
}
