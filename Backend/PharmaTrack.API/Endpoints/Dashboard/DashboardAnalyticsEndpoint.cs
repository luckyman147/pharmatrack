using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.Queries.Dashboard.Stats;
using PharmaTrack.Application.DTOs.Dashboard;

namespace PharmaTrack.API.Endpoints.Dashboard;


public class DashboardAnalyticsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/dashboard").WithTags("Dashboard Analytics");

        group.MapGet("/orders", async ([FromQuery] string? period, [FromQuery] DateTime? start, [FromQuery] DateTime? end, ISender sender) =>
        {
            var dates = ResolveDates(period, start, end);
            var result = await sender.Send(new GetOrderAnalyticsQuery(dates.Start, dates.End));
            return Results.Ok(result);
        }).WithName("GetOrderAnalytics").Produces<OrderAnalyticsDto>(200);

        group.MapGet("/inventory", async ([FromQuery] string? period, [FromQuery] DateTime? start, [FromQuery] DateTime? end, ISender sender) =>
        {
            var dates = ResolveDates(period, start, end);
            var result = await sender.Send(new GetInventoryAnalyticsQuery(dates.Start, dates.End));
            return Results.Ok(result);
        }).WithName("GetInventoryAnalytics").Produces<InventoryAnalyticsDto>(200);

        group.MapGet("/pharmacies", async ([FromQuery] string? period, [FromQuery] DateTime? start, [FromQuery] DateTime? end, ISender sender) =>
        {
            var dates = ResolveDates(period, start, end);
            var result = await sender.Send(new GetPharmacyAnalyticsQuery(dates.Start, dates.End));
            return Results.Ok(result);
        }).WithName("GetPharmacyAnalytics").Produces<PharmacyAnalyticsDto>(200);

        group.MapGet("/visits", async ([FromQuery] string? period, [FromQuery] DateTime? start, [FromQuery] DateTime? end, ISender sender) =>
        {
            var dates = ResolveDates(period, start, end);
            var result = await sender.Send(new GetVisitAnalyticsQuery(dates.Start, dates.End));
            return Results.Ok(result);
        }).WithName("GetVisitAnalytics").Produces<VisitAnalyticsDto>(200);
    }

    private static (DateTime? Start, DateTime? End) ResolveDates(string? period, DateTime? start, DateTime? end)
    {
        if (start.HasValue || end.HasValue) return (start, end);

        var now = DateTime.UtcNow;
        var p = period?.ToLower().Trim();

        return p switch
        {
            "semaine" or "week" => (GetStartOfWeek(now), now),
            "mois" or "month" => (new DateTime(now.Year, now.Month, 1), now),
            "trimestre" or "trimester" or "quarter" => (now.AddMonths(-3), now),
            "semestre" or "semester" => (now.AddMonths(-6), now), // Just in case
            "année" or "annee" or "year" => (new DateTime(now.Year, 1, 1), now),
            _ => (new DateTime(now.Year, now.Month, 1), now) // Default: Current Month
        };
    }

    private static DateTime GetStartOfWeek(DateTime date)
    {
        // Monday as start of week
        var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.AddDays(-1 * diff).Date;
    }
}
