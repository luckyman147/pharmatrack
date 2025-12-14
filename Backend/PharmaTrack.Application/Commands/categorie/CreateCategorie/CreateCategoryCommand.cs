using System;
using FluentValidation;
using PharmaTrack.Application.DTOs.Categorie;
using PharmaTrack.Domain.Models.Produit;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.categorie;

public record CreateCategoryCommand (CreateCategorieDto createCategorieDto):ICommand<CreateCategoryResult>;
public record CreateCategoryResult (Categorie categorie);

public class CreateCommandValidator:AbstractValidator<CreateCategoryCommand>


{
 public    CreateCommandValidator()
    {
        RuleFor(xc => xc.createCategorieDto.name).NotNull().NotEmpty().MaximumLength(100).NotEqual("string");
    }

}
