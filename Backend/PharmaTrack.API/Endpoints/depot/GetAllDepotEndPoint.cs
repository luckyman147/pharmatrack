using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.DTOs.depot;
using PharmaTrack.Application.Queries.DepotQuery.GetAlldepot;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.API.Endpoints.depot;
public record GetAllDepotRequest(PaginationRequest PaginationRequest, DepotFilter? Filter);
public record GetAllDepotResponse(PaginationResult<DepotSummaryInfo> Depots);
public class GetAllDepotEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/Depots", async ([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromBody] DepotFilter? filter, ISender mediator) =>
        {
            var request = new GetAllDepotRequest(new PaginationRequest(pageIndex, pageSize), filter);
            var result = await mediator.Send(new GetAllDepotQuery(request.PaginationRequest, request.Filter));
            var response = new GetAllDepotResponse(result.Depots);
            return Results.Ok(response);
        }).WithName("GetAllDepots").WithTags("Depots")
        .Produces<GetAllDepotResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithDescription("Retrieve a paginated list of depots with optional filtering.");
    }
}
