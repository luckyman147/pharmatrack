using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models.Shared;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.depot.deletedepot;

public class deleteDepotCommandHandler (IDepotRepository depotRepository, IActivityLogRepository activityRepository) : ICommandHandler<DeleteDepotCommand, DeleteDepotResult>
{
    private readonly IDepotRepository _depotRepository = depotRepository;
    private readonly IActivityLogRepository _activityRepository = activityRepository;

    public async Task<DeleteDepotResult> Handle(DeleteDepotCommand request, CancellationToken cancellationToken)
    {
        // Ideally fetch details before delete to log readable name, but ID is sufficient for now
        await _depotRepository.DeleteAsync(request.Id);

        // Activity Log
        await _activityRepository.AddAsync(new ActivityLog
        {
            Action = "DELETE_DEPOT",
            Description = $"Deleted depot {request.Id}",
            EntityId = request.Id,
            EntityType = "Depot",
            Timestamp = DateTime.UtcNow,
            UserId = Guid.Empty, // Placeholder
            UserName = "System/User",
            UserRole = "Unknown"
        });

        return new DeleteDepotResult(request.Id);
    }
}
