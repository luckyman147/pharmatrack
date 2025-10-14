using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Domain.Models.Pharamcie;

namespace PharmaTrack.API.Endpoints.PharmacieEndpoints;
public record GetPharmaciesByNameRequest(string name);
public record GetPharmaciesByNameResponse(IEnumerable<Pharmacie> pharmacies);
public class GetPharmacieByNameendPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/Pharmacies", async ([FromQuery] string name, ISender mediator) =>
        {
            var request = new GetPharmaciesByNameRequest(name);
            var result = await mediator.Send(new GetPharmacieByNameQuery(request.name));
            var response = new GetPharmaciesByNameResponse(result.Pharmacies);
            return Results.Ok(response);

        }).WithName("Get pharmaByName").WithTags("Pharmacie").
WithDescription("Get Phamacies by name").WithDisplayName("Get Pharmacies by name")
       . Produces<GetPharmaciesByNameResponse>(StatusCodes.Status200OK).ProducesProblem(StatusCodes.Status404NotFound);
    }
}
