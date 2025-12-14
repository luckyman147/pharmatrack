namespace PharmaTrack.Application.DTOs.Dashboard;

public class DelegateVisitStatDto
{
    public string DelegateName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string Id { get; set; } = string.Empty;
    public int VisitCount { get; set; }
    public double Growth { get; set; }
}
