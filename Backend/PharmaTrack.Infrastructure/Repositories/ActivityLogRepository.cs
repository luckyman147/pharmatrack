using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using PharmaTrack.Application.Data;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models.Shared;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Infrastructure.Repositories
{
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly IMongoCollection<ActivityLog> _logs;

        public ActivityLogRepository(IApplicationDbContext context)
        {
            _logs = context.ActivityLogs;
        }

        public async Task AddAsync(ActivityLog log)
        {
            await _logs.InsertOneAsync(log);
        }

        public async Task<PaginationResult<ActivityLog>> GetAllAsync(int pageIndex, int pageSize)
        {
            var totalCount = await _logs.CountDocumentsAsync(Builders<ActivityLog>.Filter.Empty);
            var sort = Builders<ActivityLog>.Sort.Descending(x => x.Timestamp);
            
            var items = await _logs.Find(Builders<ActivityLog>.Filter.Empty)
                .Sort(sort)
                .Skip(pageIndex * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return new PaginationResult<ActivityLog>(pageIndex, pageSize, totalCount, items);
        }
    }
}
