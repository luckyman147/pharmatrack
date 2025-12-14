namespace PharmaTrack.Application.DTOs.Dashboard;

public class OrderAnalyticsDto
{
    public StatCardDto Stats { get; set; } = new();
    public List<ChartDataPoint> Evolution { get; set; } = [];
    public List<TopItemDto> TopProducts { get; set; } = [];
}
