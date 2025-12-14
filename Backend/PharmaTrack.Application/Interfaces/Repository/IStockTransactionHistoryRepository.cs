using PharmaTrack.Shared.Pagination;
using PharmaTrack.Application.DTOs.StockHistory;
using PharmaTrack.Domain.Models.Shared;

namespace PharmaTrack.Application.Interfaces.Repository;

public interface IStockTransactionHistoryRepository
{
    Task AddAsync(StockTransactionHistory history);
    Task<PaginationResult<StockTransactionHistory>> GetHistoryAsync(
        StockTransactionHistoryFilter? filter = null, 
        PaginationRequest? pagination = null);
}
