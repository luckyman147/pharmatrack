using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.depot.RemoveProduitFromDepot;

public class RemoveProduitFromDepotCommandHandler (IDepotRepository depotRepository) : ICommandHandler<RemoveProduitFromDepotCommand, RemoveProduitFromDepotResult>
{
    private readonly IDepotRepository _depotRepository = depotRepository;
    public async Task<RemoveProduitFromDepotResult> Handle(RemoveProduitFromDepotCommand request, CancellationToken cancellationToken)
    {
         await _depotRepository.RemoveProduitFromDepotAsync(request.depotId, request.produitId);
        return new RemoveProduitFromDepotResult();
    }

}
