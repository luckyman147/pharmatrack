using System;
using PharmaTrack.Domain.Abstractions;
using PharmaTrack.Domain.Models.Pharamcie;
using PharmaTrack.Domain.Models.user;

namespace PharmaTrack.Domain.Models.Visite;

public class Visite : Entity<Guid>
{
    public DateTime VisitDate { get; set; } = DateTime.Now; // visitDate: DateTime
    public string Description { get; set; } = string.Empty;
    public string? ReportUrl { get; set; } // report: string (optional)
    public required PharamcieBasicInfo pharamcieBasicInfo { get; set; }
        public required ProfilebasicInfo delegueteInfo { get; set; }
}
