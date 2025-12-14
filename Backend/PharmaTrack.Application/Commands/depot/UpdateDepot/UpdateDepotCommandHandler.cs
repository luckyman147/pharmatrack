using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models.Shared;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.depot.UpdateDepot;

public class UpdateDepotCommandHandler(
IDepotRepository depotRepository,
IActivityLogRepository activityRepository
) : ICommandHandler<UpdateDepotCommand, UpdateDepotResult>
{
    private readonly IDepotRepository _depotRepository = depotRepository;
    private readonly IActivityLogRepository _activityRepository = activityRepository;

    public async Task<UpdateDepotResult> Handle(UpdateDepotCommand request, CancellationToken cancellationToken)
    {
        var depot = await _depotRepository.GetByIdAsync(request.id);
        if (depot == null)
        {
            throw new Exception("Depot not found");
        }

        depot.Name = request.dto.Name;
        depot.Telephone = request.dto.Telephone;
        depot.RapportDocumentUrl = request.dto.RapportDocumentUrl;
        depot.Location = request.dto.adress;

        await _depotRepository.UpdateAsync(depot);

        // Activity Log
        await _activityRepository.AddAsync(new ActivityLog
        {
            Action = "UPDATE_DEPOT",
            Description = $"Updated depot {depot.Name}",
            EntityId = depot.id,
            EntityType = "Depot",
            Timestamp = DateTime.UtcNow,
            // User info should ideally come from current user context service
            UserId = Guid.Empty, 
            UserName = "System/User",
            UserRole = "Unknown"
        });

        return new UpdateDepotResult(depot.id);
        
    }
}
