using System;
using FluentValidation;
using PharmaTrack.Domain.Models.Pharamcie;
using PharmaTrack.Shared.CQRS;



public record GetPharmacieByNameQuery(String name):IQuery<GetPharmacieByNameResult>;
public record GetPharmacieByNameResult(IEnumerable<Pharmacie> Pharmacies);

public class GetPharmacieByNameValidator : AbstractValidator<GetPharmacieByNameQuery>
{
    public GetPharmacieByNameValidator()
    {
        RuleFor(x => x.name).NotEmpty().NotNull();
    }
}
