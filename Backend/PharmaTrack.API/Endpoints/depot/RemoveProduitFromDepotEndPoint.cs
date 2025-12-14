using System;
using Carter;
using MediatR;
using PharmaTrack.Application.Commands.depot.RemoveProduitFromDepot;

namespace PharmaTrack.API.Endpoints.depot;
public record RemoveProduitFromDepotRequest(Guid depotId, Guid produitId);
public record RemoveProduitFromDepotResponse();
public class RemoveProduitFromDepotEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {//from query 
        app.MapDelete("/Depots/{depotId}/produit/{produitId}", async (Guid depotId, Guid produitId, ISender mediator) =>
        {
            var request = new RemoveProduitFromDepotRequest(depotId, produitId);
            var command = new RemoveProduitFromDepotCommand(request.depotId, request.produitId);
            await mediator.Send(command);
            return Results.NoContent();
        }).WithName("RemoveProduitFromDepot").WithTags("Depots")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        ;
    }
}
