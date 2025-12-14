
using PharmaTrack.Application.DTOs.produit;
using PharmaTrack.Domain.Models.Produit;

namespace PharmaTrack.Domain.Converters;

public static class ProduitConverter
{
public static Produit ToProduit(this ProduitDetailsDto? dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));
   
        return new Produit
        {
            id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            PrixVente = dto.PrixVente,
        Categorie = dto.Categorie,
numberDepots= dto.Depots.Count
        };
    }
}
