using System;

namespace PharmaTrack.Application.DTOs.Visite;

public class UpdateVisiteDto

{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public string? ReportUrl { get; set; }
   

}
