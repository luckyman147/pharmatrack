using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models.Pharamcie;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Exceptions;



public class GetPharmaciesByNameQueryHandler(IPharamcieRepository pharamcieRepository) : IQueryHandler<GetPharmacieByNameQuery, GetPharmacieByNameResult>
{
    private readonly IPharamcieRepository _phar = pharamcieRepository;
    public async Task<GetPharmacieByNameResult> Handle(GetPharmacieByNameQuery request, CancellationToken cancellationToken)
    {
        var pharmacies = await _phar.GetPharmacieByName(request.name);
        if (pharmacies == null || !pharmacies.Any())
            throw new NotFoundException("Aucun pharmacie trouvé.");
        IEnumerable<Pharmacie> pharEnumerable = pharmacies.AsEnumerable();
        return new GetPharmacieByNameResult(pharEnumerable);


    }
}
