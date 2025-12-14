using System;
using PharmaTrack.Domain.Models.shared;
using PharmaTrack.Domain.Models.user;
namespace PharmaTrack.Domain.Models.depot;

public class DepotBasicInfo
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Adress? address { get; set; }
}
