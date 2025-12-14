using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models.Commande;
using PharmaTrack.Domain.Models.depot;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Domain.enums;
using PharmaTrack.Domain.Models.Shared;

namespace PharmaTrack.Application.Commands.commande.UpdateCommande;

public class UpdateCommandeCommandHandler(ICommandeRepository commandeRepository, IDepotRepository depotRepository, IStockTransactionHistoryRepository historyRepository) 
    : ICommandHandler<UpdateCommandeCommand, UpdateCommandeResult>
{
    private readonly ICommandeRepository _commandeRepository = commandeRepository;
    private readonly IDepotRepository _depotRepository = depotRepository;
    private readonly IStockTransactionHistoryRepository _historyRepository = historyRepository;

    public async Task<UpdateCommandeResult> Handle(UpdateCommandeCommand request, CancellationToken cancellationToken)
    {
        var commande = await _commandeRepository.GetByIdAsync(request.Id);

        if (commande == null)
        {
            throw new Exception($"Commande with id {request.Id} not found");
        }

        var oldStatus = commande.Status; // capture old status
        commande.OriginalDocumentPath = request.CommandeDto.OriginalDocumentPath;
        commande.ExportedDocumentPath = request.CommandeDto.ExportedDocumentPath;

        // Only allow line updates when the commande is already validated (status = Valide).
        if (request.CommandeDto.Lines.Any() && commande.Status != StatusCommande.Valide)
        {
            commande.Lines = request.CommandeDto.Lines.Select(line => new LigneCommande
            {
                ProduitId = line.ProduitId,
                NomProuit = line.NomProuit,
                UniteProduitPrix = line.UniteProduitPrix,
                allocationsDepots = line.AllocationsDepots.Select(alloc => new AllocationDepot
                {
                    DepotInfo = new DepotBasicInfo
                    {
                        Name = alloc.DepotName,
                        address = alloc.DepotAddress,
                        Id = alloc.DepotId
                    },
                    QuantityAllocated = alloc.QuantityAllocated
                }).ToList()
            }).ToList();
            await AdjustStockAndLogAsync(commande);
        }
        // If the status is not Valide, we intentionally ignore line changes and only update document paths.

        await _commandeRepository.UpdateAsync(commande);

        return new UpdateCommandeResult(true);  
    }

    private async Task AdjustStockAndLogAsync(PharmaTrack.Domain.Models.Commande.Commande commande)
    {
        var allocations = commande.Lines
            .SelectMany(l => l.allocationsDepots.Select(a => new { l.ProduitId, DepotId = a.DepotInfo.Id, a.QuantityAllocated }))
            .GroupBy(x => x.DepotId);

        foreach (var group in allocations)
        {
            var depot = await _depotRepository.GetByIdAsync(group.Key);
            if (depot == null) continue;

            foreach (var item in group)
            {
                var productStock = depot.ProduitStock.FirstOrDefault(p => p.ProduitId == item.ProduitId);
                if (productStock != null)
                {
                    int oldQty = productStock.Quantity;
                    // Deduct allocated quantity from stock
                    productStock.Quantity -= item.QuantityAllocated;
                    var history = new StockTransactionHistory
                    {
                        DepotId = depot.id,
                        ProduitId = item.ProduitId,
                        DepotName = depot.Name,
                        ProduitName = productStock.ProduitNom,
                        CommandeId = commande.id,
                        OldQuantity = oldQty,
                        NewQuantity = productStock.Quantity,
                        Reason = TransactionReason.CommandValidated,
                        TransactionDate = DateTime.UtcNow
                    };
                    await _historyRepository.AddAsync(history);
                }
            }
        }
    }
}

