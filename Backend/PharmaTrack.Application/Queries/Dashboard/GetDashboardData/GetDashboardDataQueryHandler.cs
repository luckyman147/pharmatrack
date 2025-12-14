using PharmaTrack.Domain.Models.Pharamcie;
using PharmaTrack.Application.DTOs.Dashboard;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Pagination;
using PharmaTrack.Domain.enums;

namespace PharmaTrack.Application.Queries.Dashboard.GetDashboardData;

public class GetDashboardDataQueryHandler(
    ICommandeRepository commandeRepository,
    IDepotRepository depotRepository,
    IVisiteRepository visiteRepository,
    IPharamcieRepository pharamcieRepository,
    IProduitRepository produitRepository,
    IActivityLogRepository activityLogRepository)
    : IQueryHandler<GetDashboardDataQuery, DashboardDto>
{
    private readonly ICommandeRepository _commandeRepository = commandeRepository;
    private readonly IDepotRepository _depotRepository = depotRepository;
    private readonly IVisiteRepository _visiteRepository = visiteRepository;
    private readonly IPharamcieRepository _pharamcieRepository = pharamcieRepository;
    private readonly IProduitRepository _produitRepository = produitRepository;
    private readonly IActivityLogRepository _activityLogRepository = activityLogRepository;

    public async Task<DashboardDto> Handle(GetDashboardDataQuery request, CancellationToken cancellationToken)
    {
        var dashboard = new DashboardDto();

        // 1. Determine Date Range
        var now = DateTime.UtcNow;
        // Default to current month if no dates provided
        var start = request.StartDate ?? new DateTime(now.Year, now.Month, 1);
        var end = request.EndDate ?? now;

        // Calculate Previous Period for Growth
        var duration = end - start;
        // If duration is zero (start=end), default to 1 day for comparison or handle gracefully
        if (duration == TimeSpan.Zero) duration = TimeSpan.FromDays(1);
        
        var previousStart = start - duration;
        var previousEnd = start;

        // 2. Fetch Data (Fetch All and Filter In-Memory as per existing pattern)
        // TODO: Move filtering to Repository for performance in production
        var allCommandes = (await _commandeRepository.GetAllAsync()).ToList();
        var validCommandes = allCommandes; 

        // Filter Commandes for Selected and Previous Period
        var currentPeriodCommandes = validCommandes
            .Where(c => c.OrderDate >= start && c.OrderDate <= end)
            .ToList();

        var previousPeriodCommandes = validCommandes
            .Where(c => c.OrderDate >= previousStart && c.OrderDate < previousEnd)
            .ToList();

        // Fetch other entities (Snapshots)
        var depotsResult = await _depotRepository.GetAllPaginatedAsync(null, new PaginationRequest { PageIndex = 0, PageSize = 1000 });
        var pharmaciesResult = await _pharamcieRepository.GetFilteredPharmaciesAsync(new PharmacieFilter { PageIndex = 0, PageSize = 1 });
        var activitiesResult = await _activityLogRepository.GetAllAsync(0, 5);
        int totalVisites = 0; 
        
        // 3. Calculate Stats
        // Commandes: Value = Count in Selection. Growth = vs Previous Period.
        dashboard.CommandesStats = CalculateStatCard(
            currentPeriodCommandes.Count, 
            currentPeriodCommandes.Count, 
            previousPeriodCommandes.Count);

        // Others: Keep as Global Totals (Snapshots)
        dashboard.DepotsStats = new StatCardDto { Value = depotsResult.Count, GrowthPercentage = 0, IsPositive = true };
        dashboard.PharmaciesStats = new StatCardDto { Value = pharmaciesResult.Count, GrowthPercentage = 0, IsPositive = true };
        dashboard.VisitesStats = new StatCardDto { Value = totalVisites, GrowthPercentage = 0, IsPositive = true };

        // 4. Build Charts - Orders Evolution
        // If range > 62 days (~2 months), group by Month. Else group by Day.
        var evolutionData = new List<ChartDataPoint>();
        
        if (duration.TotalDays > 62)
        {
            var groupByMonth = currentPeriodCommandes
                .GroupBy(c => new { c.OrderDate.Year, c.OrderDate.Month })
                .ToDictionary(g => g.Key, g => g.Count());

            var monthsDiff = ((end.Year - start.Year) * 12) + end.Month - start.Month;
            for(int i = 0; i <= monthsDiff; i++) 
            {
                var d = start.AddMonths(i);
                var key = new { d.Year, d.Month };
                int val = groupByMonth.TryGetValue(key, out var count) ? count : 0;
                evolutionData.Add(new ChartDataPoint { Label = d.ToString("MMM yyyy"), Value = val });
            }
        }
        else
        {
            var groupByDay = currentPeriodCommandes
                .GroupBy(c => c.OrderDate.Date)
                .ToDictionary(g => g.Key, g => g.Count());
             
            for (var d = start.Date; d <= end.Date; d = d.AddDays(1))
            {
                int val = groupByDay.TryGetValue(d, out var count) ? count : 0;
                evolutionData.Add(new ChartDataPoint { Label = d.ToString("dd MMM"), Value = val });
            }
        }
        dashboard.OrdersEvolution = evolutionData;

        // 5. Top Products & Pharmacies (Based on FILTERED Data)
        var productQuantities = currentPeriodCommandes
            .SelectMany(c => c.Lines)
            .GroupBy(l => l.NomProuit)
            .Select(g => new TopItemDto
            {
                Name = g.Key,
                Value = g.Sum(x => x.allocationsDepots.Sum(a => a.QuantityAllocated))
            })
            .OrderByDescending(x => x.Value)
            .Take(5)
            .ToList();
        dashboard.TopOrderedProducts = productQuantities;

        var cmdsByPharmacie = currentPeriodCommandes
            .GroupBy(c => c.PharamcieInfo.name) 
            .Select(g => new TopItemDto { Name = g.Key, Value = g.Count() })
            .OrderByDescending(x => x.Value)
            .Take(5)
            .ToList();

        // Calculate percentages
        double totalVolume = cmdsByPharmacie.Sum(x => x.Value);
        if (totalVolume > 0)
        {
            foreach (var item in cmdsByPharmacie)
            {
                item.SecondaryValue = Math.Round((item.Value / totalVolume) * 100, 1);
            }
        }
        dashboard.TopPharmaciesByVolume = cmdsByPharmacie;

        // 6. Distribution (Mocked / Based on Quantities)
        var totalAllocated = productQuantities.Sum(x => x.Value);
        dashboard.ProductDepotDistribution = productQuantities.Select((p, index) => new DistributionItemDto
        {
            Label = p.Name,
            Value = p.Value,
            Percentage = totalAllocated > 0 ? Math.Round((p.Value / totalAllocated) * 100, 0) : 0,
            Color = GetColor(index)
        }).ToList();

        // 7. Bottom Section (Mocked / Global)
        dashboard.DailyReportsCount = 982;
        dashboard.SubmittedDailyReportsCount = 3;
        dashboard.RecentActivities = activitiesResult.Data.Select(a => new RecentActivityDto
        {
            Id = a.id,
            Description = a.Description ?? a.Action,
            Timestamp = a.Timestamp,
            FormattedTime = a.Timestamp.ToString("hh:mm tt")
        }).ToList();

        return dashboard;
    }

    private static StatCardDto CalculateStatCard(long total, long current, long previous)
    {
        double growth = 0;
        if (previous > 0)
        {
            growth = ((double)(current - previous) / previous) * 100;
        }
        else if (current > 0)
        {
            growth = 100;
        }

        return new StatCardDto
        {
            Value = total,
            GrowthPercentage = Math.Round(growth, 1),
            IsPositive = growth >= 0
        };
    }

    private static string GetColor(int index)
    {
        var colors = new[] { "#8884d8", "#82ca9d", "#ffc658", "#ff8042", "#0088fe" };
        return colors[index % colors.Length];
    }
}
