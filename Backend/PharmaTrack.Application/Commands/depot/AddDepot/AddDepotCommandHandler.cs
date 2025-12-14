using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models.Shared;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.depot.AddDepot;

public class AddDepotCommandHandler(
    IDepotRepository depotRepository,
    IActivityLogRepository activityRepository) : ICommandHandler<AddDepotCommand, AddDepotResult>
{
    private readonly IDepotRepository _depotRepository = depotRepository;
    private readonly IActivityLogRepository _activityRepository = activityRepository;

    public async Task<AddDepotResult> Handle(AddDepotCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        var depot = new Domain.Models.depot.Depot
        {
            id = id,
            Name = request.DepotDto.Name,
            Telephone = request.DepotDto.Telephone,
            RapportDocumentUrl = request.DepotDto.RapportDocumentUrl,
            Location = request.DepotDto.Location,
            ProduitStock = request.DepotDto.ProduitStock
        };
        await _depotRepository.AddAsync(depot);
        
        // Activity Log
        await _activityRepository.AddAsync(new ActivityLog
        {
            Action = "CREATE_DEPOT",
            Description = $"Created new depot {depot.Name}",
            EntityId = id,
            EntityType = "Depot",
            Timestamp = DateTime.UtcNow,
            UserId = Guid.Empty, // Placeholder
            UserName = "System/User",
            UserRole = "Unknown"
        });

        return new AddDepotResult(id);
        
    }
}
