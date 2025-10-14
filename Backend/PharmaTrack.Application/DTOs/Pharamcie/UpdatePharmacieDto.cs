using System;
using PharmaTrack.Domain.Models.shared;

namespace PharmaTrack.Application.DTOs.Pharamcie;

public class UpdatePharmacieDto
{
 public string Name { get; set; } = string.Empty;        // name: string
        public string OwnerName { get; set; } = string.Empty;   // ownerName:string
        public string Telephone { get; set; } = string.Empty;
   
    // telephone:string
    

    public string Horaire { get; set; } = string.Empty;

    public string PharmacieType { get; set; } = string.Empty;

        // Avoir Address (1-to-1 Composition, so Address is embedded)
        public required Adress Location { get; set; } 
}
