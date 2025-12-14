using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.Commands.commande.AddCommande;
using PharmaTrack.Application.DTOs.commande;

namespace PharmaTrack.API.Endpoints.commande;

public record AddCommandeRequest(CreateCommandeDto CommandeDto);
public record AddCommandeResponse(Guid Id);

public class AddCommandeEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/Commande/Create", async ([FromBody] CreateCommandeDto dto, ISender mediator) =>
        {
            var request = new AddCommandeRequest(dto);
            var command = new AddCommandeCommand(request.CommandeDto);
            var result = await mediator.Send(command);
            return Results.Created($"commande/{result.Id}", result.Id);
        })
        .WithName("AddCommande")
        .WithTags("Commandes")
        .Produces<AddCommandeResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create a new commande")
        .WithDescription("Creates a new order (commande) with the provided details.");
    }
}
