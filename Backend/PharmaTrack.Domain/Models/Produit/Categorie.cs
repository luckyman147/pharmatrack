using System;
using PharmaTrack.Domain.Abstractions;

namespace PharmaTrack.Domain.Models.Produit;

public class Categorie : Entity<Guid>
{
    public required string name { get; set; }
}
public class CategoryInfo
{
    public Guid id { get; set; }
    public string name { get; set; } = string.Empty;

    public static implicit operator CategoryInfo?(Categorie? v)
    {
        throw new NotImplementedException();
    }
}
