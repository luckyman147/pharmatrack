using FluentValidation;
using PharmaTrack.Domain.Models.depot;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.depot.RemoveProduitFromDepot;

public record  RemoveProduitFromDepotCommand( Guid depotId, Guid produitId):ICommand<RemoveProduitFromDepotResult>;
public record RemoveProduitFromDepotResult();

public class RemoveProduitFromDepotCommandValidator : AbstractValidator<RemoveProduitFromDepotCommand>
{
    public RemoveProduitFromDepotCommandValidator()
    {
        RuleFor(x => x.depotId).NotEqual(Guid.Empty).WithMessage("depotId is required.");
        RuleFor(x => x.produitId).NotEqual(Guid.Empty).WithMessage("produitId is required.");
    }        
    
}

