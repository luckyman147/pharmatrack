using PharmaTrack.Domain.enums;

namespace PharmaTrack.Application.DTOs.commande;

public class UpdateCommandeDto
{
   
   
    
    
    public string OriginalDocumentPath { get; set; } = string.Empty;
    public string ExportedDocumentPath { get; set; } = string.Empty;
    public List<LigneCommandeDto> Lines { get; set; } = new List<LigneCommandeDto>();
}
