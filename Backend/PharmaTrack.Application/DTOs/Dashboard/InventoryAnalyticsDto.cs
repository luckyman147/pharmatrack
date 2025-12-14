namespace PharmaTrack.Application.DTOs.Dashboard;

public class InventoryAnalyticsDto
{
    public StatCardDto Stats { get; set; } = new();
    public List<DistributionItemDto> ProductDistribution { get; set; } = [];
}
