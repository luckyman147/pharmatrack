using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models.Shared;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Exceptions;
using System;

namespace PharmaTrack.Application.Commands.Product.DeleteProductCommand;

public class DeleteProductCommandHandler(
    IProduitRepository produitRepository,
    IDepotRepository depotRepository,
    ICommandeRepository commandeRepository,
    IActivityLogRepository activityRepository) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    private readonly IProduitRepository _produitRepository = produitRepository;
    private readonly IDepotRepository _depotRepository = depotRepository;
    private readonly ICommandeRepository _commandeRepository = commandeRepository;
    private readonly IActivityLogRepository _activityRepository = activityRepository;

    public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var produit = await _produitRepository.GetProduitByIdAsync(request.Id);
        if (produit == null)
        {
            throw new NotFoundException("Product not found");
        }

        // Cascade delete from Depots
        await _depotRepository.RemoveProductFromAllDepotsAsync(request.Id);

        // Cascade delete from Commandes
        await _commandeRepository.RemoveProductFromAllCommandesAsync(request.Id);

        // Delete Product
        await _produitRepository.DeleteProduitAsync(request.Id);

        // Activity Log
        await _activityRepository.AddAsync(new ActivityLog
        {
            Action = "DELETE_PRODUCT",
            Description = $"Deleted product {produit.Name}",
            EntityId = request.Id,
            EntityType = "Product",
            Timestamp = DateTime.UtcNow,
            UserId = Guid.Empty, // Placeholder
            UserName = "System/User",
            UserRole = "Unknown"
        });

        return new DeleteProductResult(request.Id);
    }
}
