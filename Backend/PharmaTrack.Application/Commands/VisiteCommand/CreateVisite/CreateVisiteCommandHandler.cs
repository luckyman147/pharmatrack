using System;
using Microsoft.IdentityModel.Tokens;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.Visite.CreateVisite;

public class CreateVisiteCommandHandler(IVisiteRepository visiteRepository) : ICommandHandler<CreateVisiteCommand, CreateVisiteResult>
{
    private readonly IVisiteRepository _visiteRepository = visiteRepository;
    public async Task<CreateVisiteResult> Handle(CreateVisiteCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine(request.createVisiteDto.VisitDate);
        var visite = new PharmaTrack.Domain.Models.Visite.Visite
        {
            id = Guid.NewGuid(),
            VisitDate = request.createVisiteDto.VisitDate,
            Description = request.createVisiteDto.Description,
            ReportUrl = request.createVisiteDto.ReportUrl,
            pharamcieBasicInfo = request.createVisiteDto.pharamcieBasicInfo,
            delegueteInfo = request.createVisiteDto.delegueteInfo
        };

        await _visiteRepository.CreateVisite(visite);
        return new CreateVisiteResult(visite);
    }


}
