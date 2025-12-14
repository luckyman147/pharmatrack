using FluentValidation;
using PharmaTrack.Application.DTOs.produit;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.Product.CreateProduitCommand;

public record  CreateProductCommand(CreateProductDto Dto) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Dto.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Dto.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.Dto.PrixVente).GreaterThan(0).WithMessage("PrixVente must be greater than zero.");
        RuleFor(x => x.Dto.CategorieId).NotEqual(Guid.Empty).WithMessage("CategorieId is required.");
    }
}
    