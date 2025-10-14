using System;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PharmaTrack.Domain.Abstractions;
using PharmaTrack.Domain.Converters;
using PharmaTrack.Domain.enums;
using PharmaTrack.Domain.Models.shared;

namespace PharmaTrack.Domain.Models.Pharamcie;

public class Pharmacie:Entity<Guid>
{
     public string Name { get; set; } = string.Empty;        // name: string
        public string OwnerName { get; set; } = string.Empty;   // ownerName:string
        public string Telephone { get; set; } = string.Empty;
    [JsonConverter(typeof(HoraireConverter))]
    // telephone:string
       [BsonRepresentation(BsonType.String)]

    public Horaire Horaire { get; set; }
    [JsonConverter(typeof(PharmacieTypeConverter))]
    // horaire:enum
                  [BsonRepresentation(BsonType.String)]

    public PharmacieType PharmacieType { get; set; }                 // type: enum
        
        // Avoir Address (1-to-1 Composition, so Address is embedded)
        public required Adress Location { get; set; } 
}
