using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Application.Queries.DepotQuery.GetAlldepot;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Exceptions;

namespace PharmaTrack.Application.Queries.DepotQuery;

public class GetAllDepotHandler (IDepotRepository depotRepository) : IQueryHandler<GetAllDepotQuery, GetAllDepotResult>
{
    private readonly IDepotRepository _depotRepository = depotRepository;
    public async Task<GetAllDepotResult> Handle(GetAllDepotQuery request, CancellationToken cancellationToken)
    {
        var depots = await _depotRepository.GetAllPaginatedAsync(request.Filter, request.Pagination);
        if (depots == null || depots.Data == null )
        {
            throw new NotFoundException("Error retrieving depots");
        }
        return new GetAllDepotResult(depots);
    }
}
