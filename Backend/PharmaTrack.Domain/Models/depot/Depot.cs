using System;
using PharmaTrack.Domain.Abstractions;
using PharmaTrack.Domain.Models.shared;

namespace PharmaTrack.Domain.Models.depot;

public class Depot : Entity<Guid>
{
    public required string Name { get; set; }
    public string Telephone { get; set; } = string.Empty;

    public string RapportDocumentUrl { get; set; } = string.Empty;

    public required Adress Location { get; set; }
                    public List<LigneDepotProduit> ProduitStock { get; set; } = new List<LigneDepotProduit>();
}
