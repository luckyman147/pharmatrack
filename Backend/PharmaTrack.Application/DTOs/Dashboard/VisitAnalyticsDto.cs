namespace PharmaTrack.Application.DTOs.Dashboard;

public class VisitAnalyticsDto
{
    public StatCardDto Stats { get; set; } = new();
    public List<DelegateVisitStatDto> DelegateVisits { get; set; } = [];
    public List<ChartDataPoint> VisitsTrend { get; set; } = [];
    public List<TopItemDto> TopVisitedPharmacies { get; set; } = [];
}
