using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Queries.VisitesQueries.GetVisitesOfDelegue;

public class GetVisitesOfdelegueQueryHandler(IVisiteRepository visiteRepository) : IQueryHandler<GetVisitesOfDelegueQuery, GetVisitesOfDelegueResult>
{
    private readonly IVisiteRepository _visiteRepository = visiteRepository;
    public async Task<GetVisitesOfDelegueResult> Handle(GetVisitesOfDelegueQuery request, CancellationToken cancellationToken)
    {
        var visites = await _visiteRepository.GetVisitesByDeleguesId(request.visites);
        
        return new GetVisitesOfDelegueResult(visites);
    }
}
