using FluentValidation;
using PharmaTrack.Application.DTOs.depot;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Application.Queries.DepotQuery.GetAlldepot;

public record  GetAllDepotQuery(PaginationRequest Pagination, DepotFilter? Filter):IQuery<GetAllDepotResult>;
public record GetAllDepotResult(PaginationResult<DepotSummaryInfo> Depots);

public class GetAllDepotQueryValidator : AbstractValidator<GetAllDepotQuery>
{
    public GetAllDepotQueryValidator()
    {
        RuleFor(x => x.Pagination.PageIndex).GreaterThan(0).WithMessage("PageIndex must be greater than 0.");
        RuleFor(x => x.Pagination.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0.");
    }
}

