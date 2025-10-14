using System;
using PharmaTrack.Domain.Abstractions;

namespace PharmaTrack.Domain.Models.Produit;

public class Categorie:Entity<Guid>
{
public required string name { get; set; }
}
