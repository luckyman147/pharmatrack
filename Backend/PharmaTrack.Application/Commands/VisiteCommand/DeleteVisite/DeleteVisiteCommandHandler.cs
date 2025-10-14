using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.VisiteCommand.DeleteVisite;

public class DeleteVisiteCommandHandler(IVisiteRepository visiteRepository) : ICommandHandler<DeleteVisiteCommand, DeleteVisiteResult>
{
    private readonly IVisiteRepository _visiteRepository = visiteRepository;
    public async Task<DeleteVisiteResult> Handle(DeleteVisiteCommand request, CancellationToken cancellationToken)
    {
        await _visiteRepository.DeleteVisite(request.Id);
        return new DeleteVisiteResult(true);
    }

}
