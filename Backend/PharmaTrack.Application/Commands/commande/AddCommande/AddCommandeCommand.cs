using FluentValidation;
using PharmaTrack.Application.DTOs.commande;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.commande.AddCommande;

public record AddCommandeCommand(CreateCommandeDto CommandeDto) : ICommand<AddCommandeResult>;
public record AddCommandeResult(Guid Id);

public class AddCommandeCommandValidator : AbstractValidator<AddCommandeCommand>
{
    public AddCommandeCommandValidator()
    {
   
        RuleFor(x => x.CommandeDto.PharamcieInfo)
            .NotNull().WithMessage("Pharmacie info is required.");
        
        RuleFor(x => x.CommandeDto.UserInfo)
            .NotNull().WithMessage("User info is required.");
        
 
        RuleFor(x => x.CommandeDto.Lines)
            .NotEmpty().WithMessage("Order must have at least one line.");
        
        RuleForEach(x => x.CommandeDto.Lines).ChildRules(line =>
        {
            line.RuleFor(x => x.ProduitId)
                .NotEmpty().WithMessage("ProduitId is required.");
            
            line.RuleFor(x => x.NomProuit)
                .NotEmpty().WithMessage("Product name is required.");
            
            line.RuleFor(x => x.UniteProduitPrix)
                .GreaterThan(0).WithMessage("Unit price must be greater than 0.");
            
            line.RuleFor(x => x.AllocationsDepots)
                .NotEmpty().WithMessage("At least one depot allocation is required.");
            
            line.RuleForEach(x => x.AllocationsDepots).ChildRules(alloc =>
            {
                alloc.RuleFor(x => x.DepotId)
                    .NotEmpty().WithMessage("DepotId is required.");
                
                alloc.RuleFor(x => x.QuantityAllocated)
                    .GreaterThan(0).WithMessage("Quantity allocated must be greater than 0.");
            });
        });
    }
}
