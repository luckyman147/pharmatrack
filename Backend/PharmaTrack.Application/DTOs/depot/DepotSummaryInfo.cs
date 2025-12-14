using System;
using PharmaTrack.Domain.Models.shared;

namespace PharmaTrack.Application.DTOs.depot;

public class DepotSummaryInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public Adress Location { get; set; } 
    public DateTime? LastModified { get; set; }
    public int ProduitsEnRupture { get; set; } = 0;
    public int NumberOfStockTotal { get; set; } = 0;
    public int NumberOfProducts { get; set; } = 0; // Just the count
}