using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models.Produit;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Domain.Models.Shared;

namespace PharmaTrack.Application.Commands.categorie.CreateCategorie;

public class CreateCategorieCommandHandler(ICategorieRepository categorieRepository, IActivityLogRepository activityRepository) : ICommandHandler<CreateCategoryCommand, CreateCategoryResult>
{
    private readonly ICategorieRepository _categorieRepository = categorieRepository;
    private readonly IActivityLogRepository _activityRepository = activityRepository;

    public async Task<CreateCategoryResult> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Categorie
        {
            id = Guid.NewGuid(),
            name = request.createCategorieDto.name
        };
        await _categorieRepository.CreateCategorieAsync(category);

        // Activity Log
        await _activityRepository.AddAsync(new ActivityLog
        {
            Action = "CREATE_CATEGORY",
            Description = $"Created new category {category.name}",
            EntityId = category.id,
            EntityType = "Category",
            Timestamp = DateTime.UtcNow,
            UserId = Guid.Empty, // Placeholder
            UserName = "System/User",
            UserRole = "Unknown"
        });

        return new CreateCategoryResult(category);

    }
}
