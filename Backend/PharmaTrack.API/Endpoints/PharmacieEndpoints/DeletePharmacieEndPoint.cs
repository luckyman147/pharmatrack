using System;
using Carter;
using MediatR;
using PharmaTrack.Application.Commands.pharmacie.DeletePharmacie;

namespace PharmaTrack.API.Endpoints.PharmacieEndpoints;
public record DeletePharmacieRequest(Guid Id);
public record DeletePharmacieResponse();
public class DeletePharmacieEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapDelete("/Pharmacie/{id}", async (Guid id, ISender mediator) =>
        {
            var request = new DeletePharmacieRequest(id);
            await mediator.Send(new DeletePharmacieCommand(request.Id));
            var response = new DeletePharmacieResponse();
            return Results.Ok(response);
        }).WithName("Delete Pharmacie").
        Produces<DeletePharmacieResponse>(StatusCodes.Status204NoContent).WithTags("Pharmacie")
        .ProducesProblem(StatusCodes.Status404NotFound).WithSummary("Delete a pharmacie by ID");
    }
}
