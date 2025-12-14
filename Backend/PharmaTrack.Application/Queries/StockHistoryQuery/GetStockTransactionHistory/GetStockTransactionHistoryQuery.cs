using MediatR;
using PharmaTrack.Shared.Pagination;
using PharmaTrack.Application.DTOs.StockHistory;
using PharmaTrack.Domain.Models.Shared;

namespace PharmaTrack.Application.Queries.StockHistoryQuery.GetStockTransactionHistory;

public record GetStockTransactionHistoryQuery(
    StockTransactionHistoryFilter Filter, 
    PaginationRequest Pagination) : IRequest<PaginationResult<StockTransactionHistory>>;
