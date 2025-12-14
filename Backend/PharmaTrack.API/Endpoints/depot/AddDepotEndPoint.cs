using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.Commands.depot.AddDepot;
using PharmaTrack.Application.DTOs.depot;

namespace PharmaTrack.API.Endpoints.depot;
public  record AddDepotRequest(CreateDepotDto DepotDto);
public record AddDepotResponse(Guid Id);
public class AddDepotEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/Depots/Create", async ([FromBody] CreateDepotDto dto, ISender mediator) =>
        {
            var request = new AddDepotRequest(dto);
            var command = new AddDepotCommand(request.DepotDto);
            var result = await mediator.Send(command);
            return Results.Created($"depots/{result.Id}", result.Id);
        }).WithName("AddDepot").WithTags("Depots")
        .Produces<AddDepotResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create a new depot")
        .WithDescription("Creates a new depot with the provided details.");
    }
}

