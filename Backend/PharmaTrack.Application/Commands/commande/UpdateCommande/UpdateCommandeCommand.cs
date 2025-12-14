using FluentValidation;
using PharmaTrack.Application.DTOs.commande;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.commande.UpdateCommande;

public record UpdateCommandeCommand(Guid Id, UpdateCommandeDto CommandeDto) : ICommand<UpdateCommandeResult>;
public record UpdateCommandeResult(bool Success);

public class UpdateCommandeCommandValidator : AbstractValidator<UpdateCommandeCommand>
{
    public UpdateCommandeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Commande Id is required.");
        

   

 
        
        RuleForEach(x => x.CommandeDto.Lines).ChildRules(line =>
        {
            line.RuleFor(x => x.ProduitId)
                .NotEmpty().WithMessage("ProduitId is required.");
            
            line.RuleFor(x => x.UniteProduitPrix)
                .GreaterThan(0).WithMessage("Unit price must be greater than 0.");
        });
    }
}
