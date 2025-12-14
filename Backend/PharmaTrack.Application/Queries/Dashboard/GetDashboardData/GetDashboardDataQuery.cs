using PharmaTrack.Application.DTOs.Dashboard;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Queries.Dashboard.GetDashboardData;

public record GetDashboardDataQuery(DateTime? StartDate, DateTime? EndDate) : IQuery<DashboardDto>;
