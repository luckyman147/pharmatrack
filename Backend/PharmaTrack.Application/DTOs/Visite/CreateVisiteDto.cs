using System;
using PharmaTrack.Domain.Models.Pharamcie;
using PharmaTrack.Domain.Models.user;

namespace PharmaTrack.Application.DTOs.Visite;

public class CreateVisiteDto
{
    public DateTime VisitDate { get; set; } = DateTime.Now; // visitDate: DateTime
    public string Description { get; set; } = string.Empty;
    public string? ReportUrl { get; set; } // report: string (optional)
    public required PharamcieBasicInfo pharamcieBasicInfo { get; set; }
    public required ProfilebasicInfo delegueteInfo { get; set; }

}
