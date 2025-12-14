using System;
using PharmaTrack.Domain.Models.shared;

namespace PharmaTrack.Application.DTOs.depot;

public class UpdateDepotDTO
{
    public string Name { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string RapportDocumentUrl { get; set; } = string.Empty;
    public required Adress adress { get; set; }

}
