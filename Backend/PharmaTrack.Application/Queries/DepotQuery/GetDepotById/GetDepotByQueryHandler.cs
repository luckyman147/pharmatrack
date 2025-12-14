using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Queries.DepotQuery.GetDepotById;

public class GetDepotByQueryHandler (IDepotRepository depotRepository) : IQueryHandler<GetDepotByIdQuery, GetDepotByIdResult>
{
    private readonly IDepotRepository _depotRepository = depotRepository;
    public async Task<GetDepotByIdResult> Handle(GetDepotByIdQuery request, CancellationToken cancellationToken)
    {
        var depot = await _depotRepository.GetByIdAsync(request.DepotId);
        if (depot == null)
        {
            throw new KeyNotFoundException("Depot not found");
        }
        return new GetDepotByIdResult(depot);
    }
}
