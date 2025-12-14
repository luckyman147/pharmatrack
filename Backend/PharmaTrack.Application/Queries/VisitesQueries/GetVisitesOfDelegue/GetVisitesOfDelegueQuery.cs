using FluentValidation;
using PharmaTrack.Application.DTOs.Visite;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Application.Queries.VisitesQueries.GetVisitesOfDelegue;

public record  GetVisitesOfDelegueQuery(GetVisiteByDelegueDto visites):IQuery<GetVisitesOfDelegueResult>;
public record GetVisitesOfDelegueResult(PaginationResult<Object> visites);

public class GetVisitesOfDelegueValidator : AbstractValidator<GetVisitesOfDelegueQuery>
{
    public GetVisitesOfDelegueValidator()
    {
        RuleFor(x => x.visites.DelegueId).NotEmpty().WithMessage("DelegueId is required.");
        RuleFor(x => x.visites.PageIndex).NotEmpty().WithMessage("PageIndex is required.");
        RuleFor(x => x.visites.PageSize).NotEmpty().WithMessage("PageSize is required.").GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        RuleFor(x => x.visites.byName).Must(name => string.IsNullOrEmpty(name) || (!string.IsNullOrEmpty(name) && name.Length >= 3)).WithMessage("byName must be at least 3 characters long if provided.");
      
        RuleFor(x => x.visites.startDate).Must(date => !date.HasValue || date <= DateTime.Now).WithMessage("startDate must be in the past or present.");
        RuleFor(x => x.visites.endDate).Must(date => !date.HasValue || date <= DateTime.Now).WithMessage("endDate must be in the past or present.");    
       //grouped by pharmacie is optional and can be true or false so no need to validate it

    }
}
