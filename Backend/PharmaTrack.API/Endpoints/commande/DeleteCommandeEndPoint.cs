using Carter;
using MediatR;
using PharmaTrack.Application.Commands.commande.DeleteCommande;

namespace PharmaTrack.API.Endpoints.commande;

public record DeleteCommandeResponse(bool Success);

public class DeleteCommandeEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/Commande/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var command = new DeleteCommandeCommand(id);
            var result = await mediator.Send(command);
            return Results.Ok(new DeleteCommandeResponse(result.Success));
        })
        .WithName("DeleteCommande")
        .WithTags("Commandes")
        .Produces<DeleteCommandeResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete a commande")
        .WithDescription("Deletes an existing order (commande).");
    }
}
