using System;

namespace PharmaTrack.Domain.Models.Pharamcie;

public class PharamcieBasicInfo
{
    public Guid Id { get; set; }
    public required string name { get; set; }
    public string? address { get; set; }
    
}
