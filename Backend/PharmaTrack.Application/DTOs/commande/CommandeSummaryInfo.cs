using System.Text.Json.Serialization;
using PharmaTrack.Domain.enums;
using PharmaTrack.Domain.Models.Pharamcie;
using PharmaTrack.Domain.Models.user;

namespace PharmaTrack.Application.DTOs.commande;

public class CommandeSummaryInfo
{
    public Guid Id { get; set; }
    public Guid PharmacieId { get; set; }
    public PharamcieBasicInfo PharamcieInfo { get; set; } = null!;
    public ProfilebasicInfo UserInfo { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StatusCommande Status { get; set; }
    public double TotalAmount { get; set; }
    public int CodeComm { get; set; }
    public int TotalLines { get; set; }
}
