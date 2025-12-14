using Carter;
using MediatR;
using PharmaTrack.Application.Queries.CommandeQuery.GetCommandeById;
using PharmaTrack.Domain.Models.Commande;

namespace PharmaTrack.API.Endpoints.commande;

public record GetCommandeByIdResponse(Commande? Commande);

public class GetCommandeByIdEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/Commande/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var query = new GetCommandeByIdQuery(id);
            var result = await mediator.Send(query);
            
            if (result.Commande == null)
            {
                return Results.NotFound();
            }
            
            return Results.Ok(new GetCommandeByIdResponse(result.Commande));
        })
        .WithName("GetCommandeById")
        .WithTags("Commandes")
        .Produces<GetCommandeByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get commande by ID")
        .WithDescription("Retrieves a specific order (commande) by its ID.");
    }
}
