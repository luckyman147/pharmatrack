using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.pharmacie.DeletePharmacie;

public class DeletePharmacieCommandHandler(IPharamcieRepository pharamcieRepository) : ICommandHandler<DeletePharmacieCommand, DeletePharmacieResult>
{
    private readonly IPharamcieRepository _phar = pharamcieRepository;
    public async Task<DeletePharmacieResult> Handle(DeletePharmacieCommand request, CancellationToken cancellationToken)
    {
        
        await _phar.DeletePharmacie(request.Id);
        return new DeletePharmacieResult();
    }
}
