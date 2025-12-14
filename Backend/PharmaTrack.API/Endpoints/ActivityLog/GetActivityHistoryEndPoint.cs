using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.Queries.ActivityLogQuery.GetActivityHistory;
using PharmaTrack.Domain.Models.Shared;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.API.Endpoints.ActivityLog;

public record GetActivityHistoryResponse(PaginationResult<PharmaTrack.Domain.Models.Shared.ActivityLog> Result);

public class GetActivityHistoryEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/ActivityHistory", async ([FromQuery] int pageIndex, [FromQuery] int pageSize, ISender mediator) =>
        {
            var query = new GetActivityHistoryQuery(pageIndex, pageSize);
            var result = await mediator.Send(query);
            return Results.Ok(new GetActivityHistoryResponse(result));
        })
        .WithName("GetActivityHistory")
        .WithTags("ActivityHistory")
        .Produces<GetActivityHistoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithDescription("Retrieve system activity history logs.");
    }
}
