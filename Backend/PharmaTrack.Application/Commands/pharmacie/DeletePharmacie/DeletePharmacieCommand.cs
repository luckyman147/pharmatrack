using FluentValidation;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.pharmacie.DeletePharmacie;

public record  DeletePharmacieCommand(Guid Id):ICommand<DeletePharmacieResult>; 
public record  DeletePharmacieResult(); 

public class DeletePharmacieValidator:AbstractValidator<DeletePharmacieCommand>
{
    public DeletePharmacieValidator()
    {
        RuleFor(x => x.Id).NotEmpty().NotEqual(Guid.Empty);
    }
}

