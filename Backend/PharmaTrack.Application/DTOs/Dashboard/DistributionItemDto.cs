namespace PharmaTrack.Application.DTOs.Dashboard;

public class DistributionItemDto
{
    public string Label { get; set; } = string.Empty;
    public double Value { get; set; }
    public double Percentage { get; set; }
    public string Color { get; set; } = string.Empty;
}
