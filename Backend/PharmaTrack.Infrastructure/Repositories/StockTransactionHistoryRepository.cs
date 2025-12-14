using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using PharmaTrack.Application.Data;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models.Shared;
using PharmaTrack.Shared.Pagination;
using PharmaTrack.Application.DTOs.StockHistory;

namespace PharmaTrack.Infrastructure.Repositories
{
    public class StockTransactionHistoryRepository : IStockTransactionHistoryRepository
    {
        private readonly IMongoCollection<StockTransactionHistory> _history;

        public StockTransactionHistoryRepository(IApplicationDbContext context)
        {
            _history = context.StockTransactionHistories;
        }

        public async Task AddAsync(StockTransactionHistory history)
        {
            await _history.InsertOneAsync(history);
        }

        public async Task<PaginationResult<StockTransactionHistory>> GetHistoryAsync(
            StockTransactionHistoryFilter? filter = null, 
            PaginationRequest? pagination = null)
        {
            var builder = Builders<StockTransactionHistory>.Filter;
            var mongoFilter = builder.Empty;

            if (filter != null)
            {
                if (filter.DepotId.HasValue)
                    mongoFilter &= builder.Eq(x => x.DepotId, filter.DepotId.Value);

                if (filter.ProduitId.HasValue)
                    mongoFilter &= builder.Eq(x => x.ProduitId, filter.ProduitId.Value);

                if (filter.CommandeId.HasValue)
                    mongoFilter &= builder.Eq(x => x.CommandeId, filter.CommandeId.Value);
                
                if (filter.Status.HasValue)
                    mongoFilter &= builder.Eq(x => x.Reason, filter.Status.Value);

                if (filter.StartDate.HasValue)
                    mongoFilter &= builder.Gte(x => x.TransactionDate, filter.StartDate.Value);

                if (filter.EndDate.HasValue)
                    mongoFilter &= builder.Lte(x => x.TransactionDate, filter.EndDate.Value);
            }

            var totalCount = await _history.CountDocumentsAsync(mongoFilter);

            var sort = Builders<StockTransactionHistory>.Sort.Descending(x => x.TransactionDate);
            
            int pageIndex = pagination?.PageIndex ?? 0;
            int pageSize = pagination?.PageSize ?? 10;

            var items = await _history.Find(mongoFilter)
                .Sort(sort)
                .Skip(pageIndex * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return new PaginationResult<StockTransactionHistory>(
                pageIndex, 
                pageSize, 
                totalCount, 
                items);
        }
    }
}
