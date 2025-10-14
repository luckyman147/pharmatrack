using System;
using System.Data.SqlTypes;

namespace PharmaTrack.Domain.Models.depot;

public class LigneDepotProduit
{
  public Guid ProduitId { get; set; } // Reference to the 'depot' document
    public string ProduitNom { get; set; } = string.Empty;
        public int Quantity { get; set; } // Quantite:int
}
