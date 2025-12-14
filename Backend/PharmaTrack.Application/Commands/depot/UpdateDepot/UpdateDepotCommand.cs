using FluentValidation;
using PharmaTrack.Application.DTOs.depot;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.depot.UpdateDepot;

public record  UpdateDepotCommand(Guid id ,UpdateDepotDTO dto):ICommand<UpdateDepotResult>;
public record UpdateDepotResult(Guid Id);

public class UpdateDepotCommandValidator : AbstractValidator<UpdateDepotCommand>
{
    public UpdateDepotCommandValidator()
    {
        RuleFor(x => x.id).NotEqual(Guid.Empty).WithMessage("Id is required.");
        RuleFor(x => x.dto.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.dto.Telephone).NotEmpty().WithMessage("Telephone is required.");
        RuleFor(x => x.dto.adress).NotNull().WithMessage("Adress is required.").SetValidator(new AdressValidator());
    }
}


