using PharmaTrack.Application.DTOs.Dashboard;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Application.Queries.Dashboard.Stats;

public class GetInventoryAnalyticsQueryHandler(IDepotRepository depotRepository, ICommandeRepository commandeRepository) 
    : IQueryHandler<GetInventoryAnalyticsQuery, InventoryAnalyticsDto>
{
    public async Task<InventoryAnalyticsDto> Handle(GetInventoryAnalyticsQuery request, CancellationToken cancellationToken)
    {
         var depotsResult = await depotRepository.GetAllPaginatedAsync(null, new PaginationRequest { PageIndex = 0, PageSize = 1000 });
         
         // Distribution Logic (Mocked/Derived from Orders as per original logic)
         var allCommandes = (await commandeRepository.GetAllAsync()).ToList();
         var productQuantities = allCommandes
            .SelectMany(c => c.Lines)
            .GroupBy(l => l.NomProuit)
            .Select(g => new TopItemDto { Name = g.Key, Value = g.Sum(x => x.allocationsDepots.Sum(a => a.QuantityAllocated)) })
            .OrderByDescending(x => x.Value)
            .Take(5)
            .ToList();

         var totalAllocated = productQuantities.Sum(x => x.Value);
         var distribution = productQuantities.Select((p, index) => new DistributionItemDto
         {
             Label = p.Name,
             Value = p.Value,
             Percentage = totalAllocated > 0 ? Math.Round((p.Value / totalAllocated) * 100, 0) : 0,
             Color = GetColor(index)
         }).ToList();

         return new InventoryAnalyticsDto
         {
             Stats = new StatCardDto { Value = depotsResult.Count, GrowthPercentage = 0, IsPositive = true },
             ProductDistribution = distribution
         };
    }

    private static string GetColor(int index) => (new[] { "#8884d8", "#82ca9d", "#ffc658", "#ff8042", "#0088fe" })[index % 5];
}
