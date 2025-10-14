using FluentValidation;
using PharmaTrack.Application.DTOs.Pharamcie;
using PharmaTrack.Domain.enums;
using PharmaTrack.Domain.Models.Pharamcie;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.pharmacie.CreatePharamcie;

public record class CreatePharmacieCommand(CreatePharmacieDto dto) : ICommand<CreatePharmacieResult>;
public record class CreatePharmacieResult(Pharmacie Pharmacie);

public class CreatePharmacieValidator : AbstractValidator<CreatePharmacieCommand>
{
    public CreatePharmacieValidator()
    {
        RuleFor(x => x.dto.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.dto.OwnerName).NotNull().WithMessage("OwnerName is required.");
        RuleFor(x => x.dto.Telephone).NotNull().WithMessage("Telephone is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Telephone must be a valid phone number.");
        RuleFor(x => x.dto.Location).NotNull().SetValidator(new AdressValidator());
RuleFor(x => x.dto.Horaire)
            .Must(h => h == null || Enum.IsDefined(typeof(Horaire), h))
            .WithMessage("Horaire must be either 'Jour' or 'Nuit' or null.");

        RuleFor(x => x.dto.PharmacieType)
            .Must(h => h == null || Enum.IsDefined(typeof(PharmacieType), h))
            .WithMessage("Type must be either 'Pharmacie' or 'Parapharmacie' or null.");
    }
}
