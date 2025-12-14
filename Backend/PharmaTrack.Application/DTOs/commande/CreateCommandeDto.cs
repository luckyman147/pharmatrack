using PharmaTrack.Domain.enums;
using PharmaTrack.Domain.Models.Commande;
using PharmaTrack.Domain.Models.Pharamcie;
using PharmaTrack.Domain.Models.shared;
using PharmaTrack.Domain.Models.user;

namespace PharmaTrack.Application.DTOs.commande;

public class CreateCommandeDto
{
    public required PharamcieBasicInfo PharamcieInfo { get; set; }
    public required ProfilebasicInfo UserInfo { get; set; }
    public string OriginalDocumentPath { get; set; } = string.Empty;

    public List<LigneCommandeDto> Lines { get; set; } = new List<LigneCommandeDto>();
}

public class LigneCommandeDto
{
    public Guid ProduitId { get; set; }
    public string NomProuit { get; set; } = string.Empty;
    public double UniteProduitPrix { get; set; }
    public List<AllocationDepotDto> AllocationsDepots { get; set; } = new List<AllocationDepotDto>();
}

public class AllocationDepotDto
{
    public Guid DepotId { get; set; }
    public string DepotName { get; set; } = string.Empty;
    public Adress? DepotAddress { get; set; } 
    public int QuantityAllocated { get; set; }
}
