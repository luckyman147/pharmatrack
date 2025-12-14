using PharmaTrack.Application.DTOs.Dashboard;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Queries.Dashboard.Stats;

public class GetOrderAnalyticsQueryHandler(ICommandeRepository commandeRepository)
    : IQueryHandler<GetOrderAnalyticsQuery, OrderAnalyticsDto>
{
    private readonly ICommandeRepository _commandeRepository = commandeRepository;

    public async Task<OrderAnalyticsDto> Handle(GetOrderAnalyticsQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var start = request.StartDate ?? new DateTime(now.Year, now.Month, 1);
        var end = request.EndDate ?? now;

        var duration = end - start;
        if (duration == TimeSpan.Zero) duration = TimeSpan.FromDays(1);
        var previousStart = start - duration;
        var previousEnd = start;

        var allCommandes = (await _commandeRepository.GetAllAsync()).ToList();
        
        var currentPeriodCommandes = allCommandes
            .Where(c => c.OrderDate >= start && c.OrderDate <= end)
            .ToList();

        var previousPeriodCommandes = allCommandes
            .Where(c => c.OrderDate >= previousStart && c.OrderDate < previousEnd)
            .ToList();

        var stats = CalculateStatCard(
            currentPeriodCommandes.Count, 
            currentPeriodCommandes.Count, 
            previousPeriodCommandes.Count);

        // Evolution Chart
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

        // Top Products
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

        return new OrderAnalyticsDto
        {
            Stats = stats,
            Evolution = evolutionData,
            TopProducts = productQuantities
        };
    }

    private static StatCardDto CalculateStatCard(long total, long current, long previous)
    {
        double growth = 0;
        if (previous > 0) growth = ((double)(current - previous) / previous) * 100;
        else if (current > 0) growth = 100;

        return new StatCardDto
        {
            Value = total,
            GrowthPercentage = Math.Round(growth, 1),
            IsPositive = growth >= 0
        };
    }
}
