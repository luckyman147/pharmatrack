using FluentValidation;
using PharmaTrack.Application.DTOs.produit;
using PharmaTrack.Domain.Models.Produit;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Application.Queries.ProductsQuery.GetFiltredProduits;

public record  GetFiltredProductQuery(GetFiltredProductDto GetFiltredProductDto):IQuery<GetFiltredProductResult>;
public record GetFiltredProductResult(PaginationResult<Produit> PaginationResult);

public class GetFiltredProductQueryValidator:AbstractValidator<GetFiltredProductQuery>
{
    public GetFiltredProductQueryValidator()
    {
        RuleFor(x => x.GetFiltredProductDto.PageNumber).GreaterThanOrEqualTo(0).WithMessage("PageNumber must be greater than zero.");
        RuleFor(x => x.GetFiltredProductDto.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than zero.");

    }
}
