using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.DTOs.depot;
using PharmaTrack.Application.DTOs.produit;
using PharmaTrack.Application.Queries.DepotQuery.GetDepotsStockForProduct;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.API.Endpoints.depot;

public class GetDepotsStockForProductEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("Depots/product/{produitId}", async (Guid produitId, [AsParameters] PaginationRequest pagination, [AsParameters] DepotFilter filter, ISender mediator) =>
        {
            var query = new GetDepotsStockForProductQuery(produitId, pagination, filter);
            var result = await mediator.Send(query);
            return Results.Ok(result.Depots);
        })
        .WithName("GetDepotsStockForProduct")
        .WithTags("Depots")
        .Produces<PaginationResult<DepotsProduitInfo>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get depots containing a specific product")
        .WithDescription("Returns a paginated list of depots and the quantity of the specified product in each.");
    }
}
