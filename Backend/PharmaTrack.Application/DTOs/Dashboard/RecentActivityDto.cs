namespace PharmaTrack.Application.DTOs.Dashboard;

public class RecentActivityDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string FormattedTime { get; set; } = string.Empty;
}
