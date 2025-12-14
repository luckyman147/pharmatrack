using System;
using PharmaTrack.Domain.Models.depot;
using PharmaTrack.Domain.Models.shared;

namespace PharmaTrack.Application.DTOs.depot;

public class CreateDepotDto
{
    public string Name { get; set; }= string.Empty;        // name: string
    public string Telephone { get; set; } = string.Empty; // telephone: 
    // 
    public string RapportDocumentUrl { get; set; } = string.Empty; // rapportDocumentUrl: string
    public required Adress Location { get; set; } 
                        public List<LigneDepotProduit> ProduitStock { get; set; } = new List<LigneDepotProduit>();

        // location: Adress

}
