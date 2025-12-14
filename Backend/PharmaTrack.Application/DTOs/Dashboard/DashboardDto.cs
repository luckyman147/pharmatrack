namespace PharmaTrack.Application.DTOs.Dashboard;

public class DashboardDto
{
    // Top Cards
    public StatCardDto CommandesStats { get; set; } = new();
    public StatCardDto DepotsStats { get; set; } = new();
    public StatCardDto VisitesStats { get; set; } = new();
    public StatCardDto PharmaciesStats { get; set; } = new();

    // Charts
    public List<ChartDataPoint> OrdersEvolution { get; set; } = [];
    public List<TopItemDto> TopOrderedProducts { get; set; } = [];
    public List<TopItemDto> TopPharmaciesByVolume { get; set; } = [];
    
    // Visits Section
    public List<DelegateVisitStatDto> DelegateVisits { get; set; } = [];
    public List<ChartDataPoint> VisitsTrend { get; set; } = [];
    public List<TopItemDto> TopVisitedPharmacies { get; set; } = [];

    // Distribution
    public List<DistributionItemDto> ProductDepotDistribution { get; set; } = [];

    // Bottom Section
    public int DailyReportsCount { get; set; }
    public int SubmittedDailyReportsCount { get; set; }
    public List<RecentActivityDto> RecentActivities { get; set; } = [];
}
