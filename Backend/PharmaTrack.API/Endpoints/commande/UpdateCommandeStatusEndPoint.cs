using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.Commands.commande.UpdateCommandeStatus;
using PharmaTrack.Domain.enums;

namespace PharmaTrack.API.Endpoints.commande;

public record UpdateCommandeStatusRequest(StatusCommande NewStatus);
public record UpdateCommandeStatusResponse(bool Success);

public class UpdateCommandeStatusEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/Commande/{id:guid}/status", async (Guid id, [FromBody] UpdateCommandeStatusRequest request, ISender mediator) =>
        {
            var command = new UpdateCommandeStatusCommand(id, request.NewStatus);
            var result = await mediator.Send(command);
            return Results.Ok(new UpdateCommandeStatusResponse(result.IsSuccess));
        })
        .WithName("UpdateCommandeStatus")
        .WithTags("Commandes")
        .Produces<UpdateCommandeStatusResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update commande status")
        .WithDescription("Updates the status of an existing order and handles stock adjustments.");
    }
}
