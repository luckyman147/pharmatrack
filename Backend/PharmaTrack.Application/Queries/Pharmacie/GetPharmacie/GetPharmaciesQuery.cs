using System;
using FluentValidation;
using PharmaTrack.Domain.asset;
using PharmaTrack.Domain.enums;
using PharmaTrack.Domain.Models.Pharamcie;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Pagination;

public record  GetFiltredPaginatedPharmaciesQueries(PharmacieFilter filter,string? GroupBy):IQuery<GetFiltredPaginatedPharmaciesResult>;
public record  GetFiltredPaginatedPharmaciesResult(PaginationResult<Object> pharmacies);

public class GetPharamciesValidator : AbstractValidator<GetFiltredPaginatedPharmaciesQueries>
{
    public GetPharamciesValidator()
    {
        RuleFor(x => x.filter.PageIndex)
            .NotEmpty()
            .NotNull()
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.filter.PageSize)
            .NotEmpty()
            .NotNull()
            .Equal(12);

        RuleFor(x => x.filter.Horaire)
            .Must(h => h == null || Enum.IsDefined(typeof(Horaire), h))
            .WithMessage("Horaire must be either 'Jour' or 'Nuit' or null.");

        RuleFor(x => x.filter.Type)
            .Must(h => h == null || Enum.IsDefined(typeof(PharmacieType), h))
            .WithMessage("Type must be either 'Pharmacie' or 'Parapharmacie' or null.");

        RuleFor(a => a.filter.Zone)
            .Must(zone => zone == null || AdministrativeData.GetZoneData(zone) != null)
            .WithMessage(a => $"'{a.filter}' n'est pas une Zone administrative valide (ex: Nord-Est, Sud).");

        // ✅ Rule for GroupBy
        RuleFor(x => x.GroupBy)
            .Must((query, groupBy) =>
            {
                if (string.IsNullOrWhiteSpace(groupBy))
                    return true; // ok if null or empty

                var filter = query.filter;

                // Collect all applied filters (non-null)
                var appliedFilters = new List<string>();
                if (!string.IsNullOrEmpty(filter.Zone)) appliedFilters.Add("zone");
                if (!string.IsNullOrEmpty(filter.Delegation)) appliedFilters.Add("delegation");
                if (!string.IsNullOrEmpty(filter.Gouvernerat)) appliedFilters.Add("gouvernerat");
                if (filter.Horaire != null) appliedFilters.Add("horaire");
                if (filter.Type != null) appliedFilters.Add("pharmacietype");

                // Fail if GroupBy is one of the applied filters
                return !appliedFilters.Contains(groupBy.ToLower());
            })
            .WithMessage("GroupBy cannot be one of the filters already applied.");
    }
}



