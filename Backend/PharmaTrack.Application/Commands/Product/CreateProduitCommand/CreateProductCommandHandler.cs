using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models.Produit;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Domain.Models.Shared;

namespace PharmaTrack.Application.Commands.Product.CreateProduitCommand;

public class CreateProductCommandHandler(IProduitRepository produitRepository, ICategorieRepository categorieRepository, IActivityLogRepository activityRepository) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IProduitRepository _produitRepository = produitRepository;
    private readonly ICategorieRepository _categorieRepository = categorieRepository;
    private readonly IActivityLogRepository _activityRepository = activityRepository;

    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var categorie = await _categorieRepository.GetCategorieByIdAsync(request.Dto.CategorieId);
        if (categorie == null)
            throw new Exception("Categorie not found");

        var produit = new Produit
        {
            id = Guid.NewGuid(),
            Name = request.Dto.Name,
            Description = request.Dto.Description,
            PrixVente = request.Dto.PrixVente,
            Categorie = Domain.Converters.CategoryConverter.ToCategoryInfo(categorie)
        };

        await _produitRepository.CreateProduitAsync(produit);

        // Activity Log
        await _activityRepository.AddAsync(new ActivityLog
        {
            Action = "CREATE_PRODUCT",
            Description = $"Created new product {produit.Name} in category {categorie.name}",
            EntityId = produit.id,
            EntityType = "Product",
            Timestamp = DateTime.UtcNow,
            UserId = Guid.Empty, // Placeholder
            UserName = "System/User",
            UserRole = "Unknown"
        });

        return new CreateProductResult(produit.id);
    }
}
