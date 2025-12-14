using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Queries.DepotQuery.GetDepotsStockForProduct;

public class GetDepotsStockForProductQueryHandler(IDepotRepository depotRepository)
    : IQueryHandler<GetDepotsStockForProductQuery, GetDepotsStockForProductResult>
{
    public async Task<GetDepotsStockForProductResult> Handle(GetDepotsStockForProductQuery request, CancellationToken cancellationToken)
    {
        var result = await depotRepository.GetDepotsByProduitAsync(request.ProduitId, request.Filter, request.Pagination);
        return new GetDepotsStockForProductResult(result);
    }
}
