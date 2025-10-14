using System;
using MediatR;
using PharmaTrack.Application.Interfaces.Repository;

namespace PharmaTrack.Application.Commands.VisiteCommand.UpdateVisite;

public class UpdateVisiteCommandHandler(IVisiteRepository visiteRepository) : IRequestHandler<UpdateVisiteCommand, UpdateCommandResult>
{
    private readonly IVisiteRepository visiteRepository = visiteRepository;
    public async Task<UpdateCommandResult> Handle(UpdateVisiteCommand request, CancellationToken cancellationToken)
    {
        var existingVisite = await visiteRepository.Get(request.updateVisiteDto.Id);
        if (existingVisite == null)
        {
            throw new KeyNotFoundException($"Visite with Id {request.updateVisiteDto.Id} not found.");
        }

        existingVisite.VisitDate = request.updateVisiteDto.Date;
        existingVisite.Description = request.updateVisiteDto.Description;
        existingVisite.ReportUrl = request.updateVisiteDto.ReportUrl;

        await visiteRepository.UpdateVisite(existingVisite);

        return new UpdateCommandResult(existingVisite);
    }

}
