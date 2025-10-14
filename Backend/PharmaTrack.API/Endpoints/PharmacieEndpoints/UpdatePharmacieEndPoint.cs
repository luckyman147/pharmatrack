using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.Commands.pharmacie.UpdatePharamcie;
using PharmaTrack.Application.DTOs.Pharamcie;
using PharmaTrack.Domain.Models.Pharamcie;

namespace PharmaTrack.API.Endpoints.PharmacieEndpoints;
public record UpdatePharmacieRequest(Guid Id, UpdatePharmacieDto dto);
public record UpdatePharmacieResponse(Pharmacie Pharmacie);
public class UpdatePharmacieEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/pharmacies/{id}", async (Guid id, [FromBody] UpdatePharmacieDto dto, IMediator mediator) =>
        {
            var request = new UpdatePharmacieRequest(id, dto);
            var result = await mediator.Send(new UpdatePharmacieCommand(request.Id, request.dto));
            var response = new UpdatePharmacieResponse(result.Pharmacie);
            return Results.Ok(response);
        })
         .WithName("UpdatePharmacie")
         .Produces<UpdatePharmacieResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithTags("Pharmacie")
         .WithSummary("Update an existing pharmacie by ID");
    }
}
