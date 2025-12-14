using MediatR;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.Pagination;
using PharmaTrack.Domain.Models.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace PharmaTrack.Application.Queries.StockHistoryQuery.GetStockTransactionHistory;

public class GetStockTransactionHistoryQueryHandler : IRequestHandler<GetStockTransactionHistoryQuery, PaginationResult<StockTransactionHistory>>
{
    private readonly IStockTransactionHistoryRepository _repository;

    public GetStockTransactionHistoryQueryHandler(IStockTransactionHistoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginationResult<StockTransactionHistory>> Handle(GetStockTransactionHistoryQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetHistoryAsync(request.Filter, request.Pagination);
    }
}
