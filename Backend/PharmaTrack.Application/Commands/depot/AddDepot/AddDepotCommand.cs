using FluentValidation;
using PharmaTrack.Application.DTOs.depot;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.depot.AddDepot;

public record  AddDepotCommand(CreateDepotDto DepotDto) : ICommand<AddDepotResult>;
public record AddDepotResult(Guid Id);

public  class AddDepotCommandValidator : AbstractValidator<AddDepotCommand>
{
    public AddDepotCommandValidator()
    {
        RuleFor(x => x.DepotDto.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.DepotDto.Telephone).NotEmpty().WithMessage("Telephone is required.").Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Telephone must be a valid phone number.");
        RuleFor(x => x.DepotDto.Location).NotNull().SetValidator(new AdressValidator());
        RuleForEach(x => x.DepotDto.ProduitStock).ChildRules(item =>
        {
            item.RuleFor(x => x.ProduitId)
                .NotEmpty().WithMessage("ProduitId is required.");

            item.RuleFor(x => x.ProduitNom)
                .NotEmpty().WithMessage("ProduitNom is required.")
                .MaximumLength(100).WithMessage("ProduitNom must be at most 100 characters.");

            item.RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        });
    }        
    
}


