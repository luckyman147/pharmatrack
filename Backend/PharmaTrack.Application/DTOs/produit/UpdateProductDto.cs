using System;

namespace PharmaTrack.Application.DTOs.produit;

public class UpdateProductDto
{
    public string? imageUrl { get; set; }
    public string name { get; set; }= string.Empty;        // name: string
    public string description { get; set; } = string.Empty; // description: string
    public double prixVente { get; set; } = 0;                // prixVente: double
    public Guid categorieId { get; set; } = Guid.Empty;      // categorieId: Guid

}
