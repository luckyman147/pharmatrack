using System;

namespace PharmaTrack.Application.DTOs.Categorie;

public class UpdateCategoryDto
{
    public Guid Id { get; set; }
    public string name { get; set; } = string.Empty;

}
