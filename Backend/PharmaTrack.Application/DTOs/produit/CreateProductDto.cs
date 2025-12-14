using System;

namespace PharmaTrack.Application.DTOs.produit;

public class CreateProductDto
{
    public string? ImageUrl { get; set; }
    public string Name { get; set; }= string.Empty;        // name: string
    public string Description { get; set; } = string.Empty; // description: string
    public double PrixVente { get; set; } = 0;                // prixVente: double
    public Guid CategorieId { get; set; } = Guid.Empty;      // categorieId: Guid
}
