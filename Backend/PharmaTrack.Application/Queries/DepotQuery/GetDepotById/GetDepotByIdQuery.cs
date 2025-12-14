using FluentValidation;
using PharmaTrack.Domain.Models.depot;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Queries.DepotQuery.GetDepotById;

public record GetDepotByIdQuery(Guid DepotId) : IQuery<GetDepotByIdResult>;
public record GetDepotByIdResult(Depot Depot);

public class GetDepotByIdValidator : AbstractValidator<GetDepotByIdQuery>
{
    public GetDepotByIdValidator()
    {
        RuleFor(x => x.DepotId).NotEqual(Guid.Empty).WithMessage("DepotId is required.");
    }
}

