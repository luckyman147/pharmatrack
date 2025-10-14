using System;
using PharmaTrack.Domain.Models.Pharamcie;

namespace PharmaTrack.Application.DTOs.Pharamcie;

public class PharmacieGroupResult
{
    public string Key { get; set; } = string.Empty;          // e.g. "Garde", "Nord-Est", etc.
    public int Count { get; set; }                           // number of pharmacies in this group
    public IEnumerable<Pharmacie> Pharmacies { get; set; } = new List<Pharmacie>();
}
