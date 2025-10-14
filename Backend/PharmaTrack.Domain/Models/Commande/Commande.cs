using System;
using MongoDB.Bson.Serialization.Attributes;
using PharmaTrack.Domain.Abstractions;
using PharmaTrack.Domain.enums;
using PharmaTrack.Domain.Models.Pharamcie;
using PharmaTrack.Domain.Models.user;

namespace PharmaTrack.Domain.Models.Commande;

public class Commande:Entity<Guid> 
{
 public Guid PharmacieId { get; set; }

    // UML attributes
        public required PharamcieBasicInfo PharamcieInfo { get; set; }
        public required ProfilebasicInfo UserInfo { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;     // orderDate: DateTime
    public StatusCommande Status { get; set; } = StatusCommande.EnAttente;                 // status: enum
    
        public double TotalAmount
        {
            get
            {
                // Use LINQ to calculate the sum of all 'TotalAmount' properties in the 'Lines' list.
                // We use 'Lines?.Sum(line => line.TotalAmount) ?? 0' for safe access and to default to 0
                // if the Lines list is null (shouldn't happen with the initialization, but good practice).
                return Lines?.Sum(line => line.TotalLineAmount) ?? 0;
            }
            // The setter is removed to ensure it's always calculated.
        }
                   // totalAmount: double
        public int CodeComm { get; set; }                           // codeComm:int
        public string OriginalDocumentPath { get; set; } = string.Empty; // original DocumentPath
        public string ExportedDocumentPath { get; set; } = string.Empty; // exported DocumentPath

        // Avoir Ligne de commande (0..* Composition, so list is embedded)
        public List<LigneCommande> Lines { get; set; } = new List<LigneCommande>();
}
