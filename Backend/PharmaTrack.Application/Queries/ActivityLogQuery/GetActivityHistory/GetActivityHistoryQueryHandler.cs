using MediatR;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models.Shared;
using PharmaTrack.Shared.Pagination;
using System.Threading;
using System.Threading.Tasks;

namespace PharmaTrack.Application.Queries.ActivityLogQuery.GetActivityHistory;

public class GetActivityHistoryQueryHandler : IRequestHandler<GetActivityHistoryQuery, PaginationResult<ActivityLog>>
{
    private readonly IActivityLogRepository _repository;

    public GetActivityHistoryQueryHandler(IActivityLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginationResult<ActivityLog>> Handle(GetActivityHistoryQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(request.PageIndex, request.PageSize);
    }
}
