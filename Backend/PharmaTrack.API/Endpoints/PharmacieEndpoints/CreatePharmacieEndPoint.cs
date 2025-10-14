using System;
using Carter;
using MediatR;
using PharmaTrack.Application.Commands.pharmacie.CreatePharamcie;
using PharmaTrack.Application.DTOs.Pharamcie;
using PharmaTrack.Domain.Models.Pharamcie;

namespace PharmaTrack.API.Endpoints.PharmacieEndpoints;
public record CreatePharmacieRequest(CreatePharmacieDto CreatePharmacieDto);
public record CreatePharmacieResponse(Pharmacie Pharmacie);

public class CreatePharmacieEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapPost("/Create-Pharmacie", async (CreatePharmacieDto createPharmacieDto, ISender mediator) =>
        {
            var request = new CreatePharmacieRequest(createPharmacieDto);
            var result = await mediator.Send(new CreatePharmacieCommand(request.CreatePharmacieDto));
            var response = new CreatePharmacieResponse(result.Pharmacie);
            return Results.Ok(response);
        }).WithName("Create Pharmacie").WithTags("Pharmacie")
        .Produces<CreatePharmacieResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
