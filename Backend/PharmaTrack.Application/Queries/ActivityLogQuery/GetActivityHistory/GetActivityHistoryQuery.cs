using MediatR;
using PharmaTrack.Domain.Models.Shared;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Application.Queries.ActivityLogQuery.GetActivityHistory;

public record GetActivityHistoryQuery(int PageIndex, int PageSize) : IRequest<PaginationResult<ActivityLog>>;
