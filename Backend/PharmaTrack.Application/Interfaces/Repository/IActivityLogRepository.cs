using System;
using System.Threading.Tasks;
using PharmaTrack.Domain.Models.Shared;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Application.Interfaces.Repository;

public interface IActivityLogRepository
{
    Task AddAsync(ActivityLog log);
    Task<PaginationResult<ActivityLog>> GetAllAsync(int pageIndex, int pageSize);
}
