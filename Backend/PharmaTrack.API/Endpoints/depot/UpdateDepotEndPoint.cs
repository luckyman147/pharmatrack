using System;
using Carter;
using MediatR;
using PharmaTrack.Application.Commands.depot.UpdateDepot;
using PharmaTrack.Application.DTOs.depot;

namespace PharmaTrack.API.Endpoints.depot;
public record UpdateDepotRequest(Guid Id, UpdateDepotDTO Dto);
public record UpdateDepotResponse(Guid Id);
public class UpdateDepotEndPoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/Depots/{id}", async (Guid id, UpdateDepotRequest request, ISender mediator) =>
        {
            var command = new UpdateDepotCommand(id, request.Dto);
            await mediator.Send(command);
            return Results.NoContent();
        }).WithName("UpdateDepot").WithTags("Depots")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
