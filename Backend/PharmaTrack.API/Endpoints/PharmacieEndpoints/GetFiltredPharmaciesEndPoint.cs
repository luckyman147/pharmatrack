using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Domain.Models.Pharamcie;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.API.Endpoints.PharmacieEndPoints;
public record GetFiltredPharmacieRequest(PharmacieFilter Filter,string? GroupedBy=null);
public record GetFiltredPharmacieResponse(PaginationResult<Object> pharmacies);

public class GetFiltredPharmaciesEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
      app.MapPost("/Pharmacies",  async ([FromBody] PharmacieFilter filter,[FromQuery] string? GroupedBy, ISender mediator,HttpContext context) =>
        {
            var request = new GetFiltredPharmacieRequest(filter,GroupedBy);
            var result = await mediator.Send(new GetFiltredPaginatedPharmaciesQueries(request.Filter,request.GroupedBy));
            var response = new GetFiltredPharmacieResponse(result.pharmacies);
            return Results.Ok(response.pharmacies);

        }).WithName("Get Pharmacies by pagination and filters").Produces<GetFiltredPharmacieResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound).WithTags("Pharmacie");
        
    }
}
