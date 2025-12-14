using System;
using Carter;
using MediatR;
using PharmaTrack.Application.Commands.depot.deletedepot;

namespace PharmaTrack.API.Endpoints.depot;
public record DeleteDepotRequest(Guid Id);
public record DeleteDepotResponse();
public class DeleteDepotEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/Depots/{id}", async (Guid id, ISender mediator) =>
        {
            var request = new DeleteDepotRequest(id);
            var command = new DeleteDepotCommand(request.Id);
            await mediator.Send(command);
            return Results.NoContent();
        }).WithName("DeleteDepot").WithTags("Depots")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        ;
    }
}
