using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Queries.Pharmacie.GroupPharmaciesByName;

public class GroupPharmacieQueryHandler(IPharamcieRepository pharamcieRepository) : IQueryHandler<GroupPharmacieQuery, GroupPharmacieResult>
{
    private readonly IPharamcieRepository _phar = pharamcieRepository;
    public async Task<GroupPharmacieResult> Handle(GroupPharmacieQuery request, CancellationToken cancellationToken)
    {
        var pharmacies = await _phar.GroupPharmacieByAsync(request.GroupBy);
        if (pharmacies == null || !pharmacies.Any())
            throw new Exception("Aucun pharmacie trouvé.");
        return new GroupPharmacieResult(pharmacies);

    }
}
