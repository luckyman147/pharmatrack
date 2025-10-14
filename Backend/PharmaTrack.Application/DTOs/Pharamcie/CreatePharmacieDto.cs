using System;
using PharmaTrack.Domain.Models.shared;

namespace PharmaTrack.Application.DTOs.Pharamcie;

public class CreatePharmacieDto
{
 public string Name { get; set; } = string.Empty;        // name: string
        public string OwnerName { get; set; } = string.Empty;   // ownerName:string
        public string Telephone { get; set; } = string.Empty;

    public string Horaire { get; set; }="Jour";

    public string PharmacieType { get; set; } = "Pharmacie";               // type: enum
        
        // Avoir Address (1-to-1 Composition, so Address is embedded)
        public required Adress Location { get; set; } 
}
