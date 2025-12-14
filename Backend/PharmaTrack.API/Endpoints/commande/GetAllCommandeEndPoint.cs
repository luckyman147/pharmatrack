using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.DTOs.commande;
using PharmaTrack.Application.Queries.CommandeQuery.GetAllCommande;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.API.Endpoints.commande;

public record GetAllCommandeRequest(PaginationRequest PaginationRequest, CommandeFilter? Filter);
public record GetAllCommandeResponse(PaginationResult<CommandeSummaryInfo> Commandes);

public class GetAllCommandeEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/commandes", async (
            [FromQuery] int pageIndex, 
            [FromQuery] int pageSize, 
            [FromBody] CommandeFilter? filter, 
            ISender mediator) =>
        {
            var request = new GetAllCommandeRequest(new PaginationRequest(pageIndex, pageSize), filter);
            var result = await mediator.Send(new GetAllCommandeQuery(request.PaginationRequest, request.Filter));
            var response = new GetAllCommandeResponse(result.Commandes);
            return Results.Ok(response);
        })
        .WithName("GetAllCommandes")
        .WithTags("Commandes")
        .Produces<GetAllCommandeResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithDescription("Retrieve a paginated list of commandes with optional filtering.");
    }
}
