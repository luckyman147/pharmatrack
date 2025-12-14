using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.Queries.Dashboard.GetDashboardData;

namespace PharmaTrack.API.Endpoints.Dashboard;

public class DashboardDataEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("dashboard", async ([FromQuery] DateTime? start, [FromQuery] DateTime? end, ISender sender) =>
        {
            var query = new GetDashboardDataQuery(start, end);
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .WithTags("Dashboard")
        .WithName("GetDashboardData")
        .Produces<Application.DTOs.Dashboard.DashboardDto>(200);
    }
}
