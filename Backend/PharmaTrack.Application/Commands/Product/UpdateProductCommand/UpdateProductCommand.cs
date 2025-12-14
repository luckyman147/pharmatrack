using FluentValidation;
using PharmaTrack.Application.DTOs.produit;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.Product.UpdateProductCommand;

public record  UpdateProductCommand(Guid Id, UpdateProductDto Dto) : ICommand<UpdateProductResult>;

public record UpdateProductResult(Guid Id);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty).WithMessage("Id is required.");
        RuleFor(x => x.Dto.name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Dto.description).NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.Dto.prixVente).GreaterThan(0).WithMessage("PrixVente must be greater than zero.");
        RuleFor(x => x.Dto.categorieId).NotEqual(Guid.Empty).WithMessage("CategorieId is required.");
    }
}
