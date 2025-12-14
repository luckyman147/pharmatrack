using System;
using PharmaTrack.Domain.Models.depot;
using PharmaTrack.Domain.Models.Produit;

namespace PharmaTrack.Application.DTOs.produit;

public class DepotsProduitInfo
{
    public Guid DepotId { get; set; }
    public string DepotName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class ProduitWithDepotCount : Produit
{
    public List<Depot> Depots { get; set; } = new();
    public int numberDepots { get; set; }
}
