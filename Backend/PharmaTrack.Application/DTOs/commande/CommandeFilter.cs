using PharmaTrack.Domain.enums;

namespace PharmaTrack.Application.DTOs.commande;

public class CommandeFilter
{
    public Guid? PharmacieId { get; set; }
    public StatusCommande? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public double? MinAmount { get; set; }
    public double? MaxAmount { get; set; }
    public int? CodeComm { get; set; }
}
