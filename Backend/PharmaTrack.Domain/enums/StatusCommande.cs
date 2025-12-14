using System.Text.Json.Serialization;

namespace PharmaTrack.Domain.enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StatusCommande
{
    EnAttente,       // Pending
    Valide,
    EnRetour,
    Annulee          // Cancelled
}
