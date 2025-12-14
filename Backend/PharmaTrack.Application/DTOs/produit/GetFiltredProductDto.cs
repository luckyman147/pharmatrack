using System;

namespace PharmaTrack.Application.DTOs.produit;

public class GetFiltredProductDto
{
    public ProduitFilter Filter { get; set; } = new ProduitFilter();
public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
