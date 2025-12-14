using PharmaTrack.Application.DTOs.Dashboard;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Queries.Dashboard.Stats;

public record GetOrderAnalyticsQuery(DateTime? StartDate, DateTime? EndDate) : IQuery<OrderAnalyticsDto>;
public record GetInventoryAnalyticsQuery(DateTime? StartDate, DateTime? EndDate) : IQuery<InventoryAnalyticsDto>;
public record GetPharmacyAnalyticsQuery(DateTime? StartDate, DateTime? EndDate) : IQuery<PharmacyAnalyticsDto>;
public record GetVisitAnalyticsQuery(DateTime? StartDate, DateTime? EndDate) : IQuery<VisitAnalyticsDto>;
