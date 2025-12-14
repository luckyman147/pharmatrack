using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.DTOs.produit;
using PharmaTrack.Application.Queries.ProductsQuery.GetFiltredProduits;
using PharmaTrack.Domain.Models.Produit;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.API.Endpoints.Produits;

public record GetProductsRequest(GetFiltredProductDto GetFiltredProductDto);
public record GetProductsResponse(PaginationResult<Produit> PaginationResult);
public class GetProducts : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapPost("/Getproducts", async ([FromQuery] int pageIndex, [FromQuery] int pageSize , GetFiltredProductDto dto, ISender mediator) =>
        {
            var request = new GetProductsRequest(new GetFiltredProductDto
            {
                Filter = dto.Filter,
                PageNumber = pageIndex,
                PageSize = pageSize
            });

            var query = new GetFiltredProductQuery(request.GetFiltredProductDto);
            var result = await mediator.Send(query);
            return new GetProductsResponse(result.PaginationResult);
        }).WithName("GetProducts").WithTags("Products")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        ;
    }
}
