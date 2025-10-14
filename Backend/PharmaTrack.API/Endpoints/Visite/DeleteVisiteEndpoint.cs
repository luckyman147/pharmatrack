using System;
using Carter;
using MediatR;
using PharmaTrack.Application.Commands.VisiteCommand.DeleteVisite;

namespace PharmaTrack.API.Endpoints.Visite;

public record DeleteVisiteRequest(Guid Id);
public record DeleteVisiteResponse(bool IsDeleted);
public class DeleteVisiteEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapDelete("/visites/{id}", async (Guid id, ISender mediator) =>
        {
            var request = new DeleteVisiteRequest(id);
            var result = await mediator.Send(new DeleteVisiteCommand(request.Id));
            var response = new DeleteVisiteResponse(result.IsDeleted);
            if (!response.IsDeleted)
            {
                return Results.NotFound($"Visite with ID {id} not found.");
            }
            return Results.NoContent();
        }).WithName("Delete Visite").WithTags("Visite")
        .Produces<DeleteVisiteResponse>(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
