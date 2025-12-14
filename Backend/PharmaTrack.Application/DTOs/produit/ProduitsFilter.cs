using System;

namespace PharmaTrack.Application.DTOs.produit;


    public class ProduitFilter
    {
        public Guid? CategorieId { get; set; }
        public string? Name { get; set; }
        public string CategorieName { get; set; } = string.Empty;
        public double? MinPrixVente { get; set; }
        public double? MaxPrixVente { get; set; }
    }

