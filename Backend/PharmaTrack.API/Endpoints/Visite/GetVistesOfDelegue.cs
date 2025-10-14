using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.DTOs.Visite;
using PharmaTrack.Application.Queries.VisitesQueries.GetVisitesOfDelegue;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.API.Endpoints.Visite;


public record GetVisitesOfDelegueRequest(GetVisiteByDelegueDto visites);
public record GetVisitesOfDelegueResponse(PaginationResult<Object> visites);

public class GetVistesOfDelegue : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapPost("/visites/delegue", static async ([FromQuery]int PageIndex, [FromQuery]int PageSize,[FromBody] GetVisiteByDelegueDto request, CancellationToken cancellationToken, ISender mediator) =>
        {
            request.PageIndex = PageIndex;
            request.PageSize = PageSize;
            var requestDto = new GetVisitesOfDelegueRequest(request);
            var query = new GetVisitesOfDelegueQuery(requestDto.visites);
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(new GetVisitesOfDelegueResponse(result.visites));
        })
        .WithTags("Visite")
        .WithName("GetVisitesOfDelegue")
        .WithSummary("Get visites of a specific delegue with optional filters and pagination.")
        .WithDescription("Retrieves visites associated with a specific delegue. Supports optional filtering by date range and report URL presence, as well as pagination.")
        .Produces<GetVisitesOfDelegueResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem();
    }
}
