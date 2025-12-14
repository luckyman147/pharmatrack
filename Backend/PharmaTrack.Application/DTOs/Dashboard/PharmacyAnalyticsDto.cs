namespace PharmaTrack.Application.DTOs.Dashboard;

public class PharmacyAnalyticsDto
{
    public StatCardDto Stats { get; set; } = new();
    public List<TopItemDto> TopPharmaciesByVolume { get; set; } = [];
}
