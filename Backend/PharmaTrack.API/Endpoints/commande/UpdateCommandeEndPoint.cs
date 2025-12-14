using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.Commands.commande.UpdateCommande;
using PharmaTrack.Application.DTOs.commande;

namespace PharmaTrack.API.Endpoints.commande;

public record UpdateCommandeRequest(Guid Id, UpdateCommandeDto CommandeDto);
public record UpdateCommandeResponse(bool Success);

public class UpdateCommandeEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/Commande/{id:guid}", async (Guid id, [FromBody] UpdateCommandeDto dto, ISender mediator) =>
        {
            var command = new UpdateCommandeCommand(id, dto);
            var result = await mediator.Send(command);
            return Results.Ok(new UpdateCommandeResponse(result.Success));
        })
        .WithName("UpdateCommande")
        .WithTags("Commandes")
        .Produces<UpdateCommandeResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update an existing commande")
        .WithDescription("Updates an existing order (commande) with the provided details.");
    }
}
