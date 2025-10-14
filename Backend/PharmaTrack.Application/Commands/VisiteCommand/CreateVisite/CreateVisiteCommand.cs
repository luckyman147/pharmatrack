using FluentValidation;
using PharmaTrack.Application.DTOs.Visite;
using PharmaTrack.Domain.Models.Visite;
using PharmaTrack.Shared.CQRS;

public record CreateVisiteCommand(CreateVisiteDto createVisiteDto) : ICommand<CreateVisiteResult>;
public record CreateVisiteResult(Visite Visite);

public class CreateVisiteCommandValidator : AbstractValidator<CreateVisiteCommand>
{
    public CreateVisiteCommandValidator()
    {
        RuleFor(x => x.createVisiteDto).NotNull();
        RuleFor(x => x.createVisiteDto.delegueteInfo).NotNull();
        RuleFor(x => x.createVisiteDto.delegueteInfo.Id).NotNull().NotEmpty().Must(id => id != Guid.Empty);
        RuleFor(x => x.createVisiteDto.pharamcieBasicInfo).NotNull();
        RuleFor(x => x.createVisiteDto.pharamcieBasicInfo.Id).NotNull().NotEmpty().Must(id => id != Guid.Empty);

        RuleFor(x => x.createVisiteDto.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.createVisiteDto.VisitDate).LessThanOrEqualTo(DateTime.Now);
    }
}


