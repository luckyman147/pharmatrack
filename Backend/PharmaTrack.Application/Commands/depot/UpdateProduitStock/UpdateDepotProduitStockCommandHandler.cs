using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.depot.UpdateProduitStock;

public class UpdateDepotProduitStockCommandHandler( IDepotRepository depotRepository) : ICommandHandler<UpdateDepotProduitStockCommand, UpdateDepotProduitStockResult>
{
    private readonly IDepotRepository _depotRepository = depotRepository;
    public async Task<UpdateDepotProduitStockResult> Handle(UpdateDepotProduitStockCommand request, CancellationToken cancellationToken)
    {
         await _depotRepository.UpdateProduitStockAsync(request.depotId, request.LigneDepotProduit);
        return new UpdateDepotProduitStockResult(request.LigneDepotProduit);
    }

}
