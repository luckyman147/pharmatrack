using FluentValidation;
using PharmaTrack.Application.DTOs.Pharamcie;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Queries.Pharmacie.GroupPharmaciesByName;

public record class GroupPharmacieQuery(String GroupBy):IQuery<GroupPharmacieResult>;
public record class GroupPharmacieResult(IEnumerable<PharmacieGroupResult> Groups);

public class GroupPharmacieValidator:AbstractValidator<GroupPharmacieQuery>
{
    public GroupPharmacieValidator()
    {
     var allowedNames = new List<string>
        {
            "zone",
            "delegation",
            "gouvernerat",
            "horaire",
            "pharmacietype"
        };

        RuleFor(x => x.GroupBy)
            .NotEmpty()
            .NotNull()
            .Must(name => allowedNames.Contains(name))
            .WithMessage($"Name must be one of the following: {string.Join(", ", allowedNames)}");
        }
}

