using FluentValidation;
using PharmaTrack.Application.DTOs.Visite;
using PharmaTrack.Domain.Models.Visite;
using PharmaTrack.Shared.CQRS;

public record  UpdateVisiteCommand(UpdateVisiteDto updateVisiteDto) : ICommand<UpdateCommandResult>;
public record UpdateCommandResult(Visite visiteDto);

public class UpdateVisiteValidator : AbstractValidator<UpdateVisiteCommand>
{
    public UpdateVisiteValidator()
    {
        RuleFor(x => x.updateVisiteDto.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.updateVisiteDto.Date).NotEmpty().Must(date => date <= DateTime.Now).WithMessage("Date must be in the past or present.");
        RuleFor(x => x.updateVisiteDto.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.updateVisiteDto.ReportUrl).
        Must(url => string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute)).WithMessage("ReportUrl must be a valid URL.");             
    }
}