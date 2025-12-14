using FluentValidation;
using PharmaTrack.Application.DTOs.depot;
using PharmaTrack.Application.DTOs.produit;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Application.Queries.DepotQuery.GetDepotsStockForProduct;

public record GetDepotsStockForProductQuery(Guid ProduitId, PaginationRequest Pagination, DepotFilter? Filter) 
    : IQuery<GetDepotsStockForProductResult>;

public record GetDepotsStockForProductResult(PaginationResult<DepotsProduitInfo> Depots);

public class GetDepotsStockForProductQueryValidator : AbstractValidator<GetDepotsStockForProductQuery>
{
    public GetDepotsStockForProductQueryValidator()
    {
        RuleFor(x => x.ProduitId).NotEmpty().WithMessage("Product ID is required.");
    }
}
