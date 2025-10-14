using System;
using Carter;
using MediatR;
using PharmaTrack.Application.DTOs.Visite;
using PharmaTrack.Domain.Models.Visite;


public record UpdateCommandRequest(UpdateVisiteDto updateVisiteDto) : IRequest<UpdateCommandResponse>;
public record UpdateCommandResponse(Visite visite);

public class UpdateVisiteEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
     
         app.MapPut("/visites", async (UpdateVisiteDto updateVisiteDto, IMediator mediator) =>
          {
                var command = new UpdateVisiteCommand(updateVisiteDto);
                var result = await mediator.Send(command);
                return Results.Ok(result);
          })
          .WithTags("Visite")
          .WithName("UpdateVisite")
          .Produces<UpdateCommandResponse>(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status400BadRequest)
          .Produces(StatusCodes.Status404NotFound);
    }
}
