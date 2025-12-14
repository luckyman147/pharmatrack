using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Exceptions;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Application.Queries.ProductsQuery.GetFiltredProduits;

public class GetFiltredProductHandler(IProduitRepository produitRepository) : IQueryHandler<GetFiltredProductQuery, GetFiltredProductResult>
{
      private readonly IProduitRepository _produitRepository = produitRepository;
    public async Task<GetFiltredProductResult> Handle(GetFiltredProductQuery request, CancellationToken cancellationToken)
    {
var paginationResult = new PaginationRequest
        {
            PageIndex = request.GetFiltredProductDto.PageNumber,
            PageSize = request.GetFiltredProductDto.PageSize
        };
        var produits = await _produitRepository.GetFilteredProduitsAsync(request.GetFiltredProductDto.Filter, paginationResult);
        if (produits == null || produits.Count == 0)
        {
            throw new NotFoundException("No products found matching the filter criteria.");
        }
        return new GetFiltredProductResult(produits);
    }
}
