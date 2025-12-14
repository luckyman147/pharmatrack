using System;
using PharmaTrack.Domain.Models.Produit;

namespace PharmaTrack.Application.DTOs.produit;

public class ProduitDetailsDto
{
public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double PrixVente { get; set; }
    public string? ImageUrl { get; set; }
    public CategoryInfo Categorie { get; set; } = new();
    public List<DepotsProduitInfo> Depots { get; set; } = new();
}
