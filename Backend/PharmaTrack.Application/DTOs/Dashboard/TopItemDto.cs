namespace PharmaTrack.Application.DTOs.Dashboard;

public class TopItemDto
{
    public string Name { get; set; } = string.Empty;
    public double Value { get; set; }
    public double? SecondaryValue { get; set; } // For percentages if needed
}
