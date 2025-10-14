using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.DTOs.Visite;
using PharmaTrack.Domain.Models.Visite;


public record CreateVisiteRequest(CreateVisiteDto CreateVisiteDto);
public record CreateVisiteResponse(Visite Visite);

public class CreateVisiteEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/visites", async ([FromBody] CreateVisiteDto createVisiteDto, IMediator mediator) =>
        {
            var request = new CreateVisiteRequest(createVisiteDto);
            var result = await mediator.Send(new CreateVisiteCommand(request.CreateVisiteDto));
            return Results.Created($"/visites/{result.Visite.id}", new CreateVisiteResponse(result.Visite));
        }).WithName("CreateVisite")
        .Produces<CreateVisiteResponse>(StatusCodes.Status201Created).
        ProducesProblem(StatusCodes.Status400BadRequest).WithSummary("Create a new visite record")
        .WithTags("Visite");
    }
}
