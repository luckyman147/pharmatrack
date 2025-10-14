using System;
using MongoDB.Bson.Serialization.Attributes;
using PharmaTrack.Domain.Models.depot;

namespace PharmaTrack.Domain.Models.Commande;

public class LigneCommande
{
  public Guid ProduitId { get; set; }

        // Added as a denormalized field for convenience (good for NoSQL read performance)
        public string NomProuit { get; set; } = string.Empty;

        // --- Fields used for the calculation ---
        
        // Mapped to 'total Quantity: int' in UML
     [BsonIgnore] // Do not store this calculated value in MongoDB
    public int TotalQuantite
    {
        get
        {
            // The constraint logic: sum (allocationsDepots.QuantityAllocated)
            return allocationsDepots?.Sum(a => a.QuantityAllocated) ?? 0;
        }
    }

        // Mapped to 'unitPrice: double' in UML
        public double UniteProduitPrix { get; set; }


        [BsonIgnore]
        public double TotalLineAmount 
        {
            get
            {
                // The calculation logic: unitPrice * total Quantity
                return UniteProduitPrix * TotalQuantite;
            }
        }
    public List<AllocationDepot> allocationsDepots { get; set; } = new List<AllocationDepot>();
}
