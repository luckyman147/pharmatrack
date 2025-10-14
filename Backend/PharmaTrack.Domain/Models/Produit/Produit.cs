using System;
using PharmaTrack.Domain.Abstractions;

namespace PharmaTrack.Domain.Models.Produit;

public class Produit : Entity<Guid>
{
      public string Name { get; set; } = string.Empty;        // name: string
        public string Description { get; set; } = string.Empty; // description: string
        public double PrixVente { get; set; }  

}
