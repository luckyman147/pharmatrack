using FluentValidation;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.commande.DeleteCommande;

public record DeleteCommandeCommand(Guid Id) : ICommand<DeleteCommandeResult>;
public record DeleteCommandeResult(bool Success);

public class DeleteCommandeCommandValidator : AbstractValidator<DeleteCommandeCommand>
{
    public DeleteCommandeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Commande Id is required.");
    }
}
