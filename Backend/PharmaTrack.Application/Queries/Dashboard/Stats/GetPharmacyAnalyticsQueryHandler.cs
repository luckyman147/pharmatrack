using PharmaTrack.Application.DTOs.Dashboard;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Domain.Models.Pharamcie;

namespace PharmaTrack.Application.Queries.Dashboard.Stats;

public class GetPharmacyAnalyticsQueryHandler(IPharamcieRepository pharamcieRepository, ICommandeRepository commandeRepository) 
    : IQueryHandler<GetPharmacyAnalyticsQuery, PharmacyAnalyticsDto>
{
    public async Task<PharmacyAnalyticsDto> Handle(GetPharmacyAnalyticsQuery request, CancellationToken cancellationToken)
    {
        var pharmaciesResult = await pharamcieRepository.GetFilteredPharmaciesAsync(new PharmacieFilter { PageIndex = 0, PageSize = 1 });
        
        // Top Pharmacies by Volume (From Orders)
        var allCommandes = (await commandeRepository.GetAllAsync()).ToList();
        var now = DateTime.UtcNow;
        var start = request.StartDate ?? new DateTime(now.Year, now.Month, 1);
        var end = request.EndDate ?? now;

        var cmdByPharmacie = allCommandes
            .Where(c => c.OrderDate >= start && c.OrderDate <= end)
            .GroupBy(c => c.PharamcieInfo.name)
            .Select(g => new TopItemDto { Name = g.Key, Value = g.Count() })
            .OrderByDescending(x => x.Value)
            .Take(5)
            .ToList();

        // Percentages
        double totalVolume = cmdByPharmacie.Sum(x => x.Value);
        if (totalVolume > 0)
        {
             foreach (var item in cmdByPharmacie)
                item.SecondaryValue = Math.Round((item.Value / totalVolume) * 100, 1);
        }

        return new PharmacyAnalyticsDto
        {
            Stats = new StatCardDto { Value = pharmaciesResult.Count, GrowthPercentage = 0, IsPositive = true },
            TopPharmaciesByVolume = cmdByPharmacie
        };
    }
}
