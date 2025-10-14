using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Exceptions;

namespace PharmaTrack.Application.Queries.Pharmacie.GetPharmacie;

public class GetAllPharmaciesQueryHandler(IPharamcieRepository pharamcieRepository) : IQueryHandler<GetFiltredPaginatedPharmaciesQueries, GetFiltredPaginatedPharmaciesResult>
{
    private readonly IPharamcieRepository _phar = pharamcieRepository;
    public async Task<GetFiltredPaginatedPharmaciesResult> Handle(GetFiltredPaginatedPharmaciesQueries request, CancellationToken cancellationToken)
    {
        Console.WriteLine("sssss");
        var pharmacies = await _phar.GetPharmaciesAsync(request.filter,request.GroupBy);
        if (pharmacies == null || pharmacies.Count == 0)
            throw new NotFoundException("Acun pharmacie");
        return new GetFiltredPaginatedPharmaciesResult(pharmacies);
    }
}
