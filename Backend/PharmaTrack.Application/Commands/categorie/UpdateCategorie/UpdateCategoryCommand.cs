using FluentValidation;
using PharmaTrack.Application.DTOs.Categorie;
using PharmaTrack.Domain.Models.Produit;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.categorie.UpdateCategorie;

public record  UpdateCategoryCommand(UpdateCategoryDto UpdateCategoryDto):ICommand<UpdateCategoryResult>;

public record UpdateCategoryResult (Categorie Categorie);

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.UpdateCategoryDto.name).NotNull().NotEmpty().MaximumLength(111);
     
    }

}