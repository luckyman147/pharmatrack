using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models.Pharamcie;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.pharmacie.CreatePharamcie;

public class CreatePharmaciCommandHandler(IPharamcieRepository pharamcieRepository) : ICommandHandler<CreatePharmacieCommand, CreatePharmacieResult>
{
    private readonly IPharamcieRepository _phar = pharamcieRepository;
    public async Task<CreatePharmacieResult> Handle(CreatePharmacieCommand request, CancellationToken cancellationToken)
    {
        var _id = Guid.NewGuid();
        var pharmacie = new Pharmacie
        {


            id = _id,
            Name = request.dto.Name,
            OwnerName = request.dto.OwnerName,
            Telephone = request.dto.Telephone,
            Location = request.dto.Location,
            Horaire = Enum.TryParse<Domain.enums.Horaire>(request.dto.Horaire, out var horaire) ? horaire : Domain.enums.Horaire.Jour,
            PharmacieType = Enum.TryParse<Domain.enums.PharmacieType>(request.dto.PharmacieType, out var type) ? type : Domain.enums.PharmacieType.Pharmacie
        };
        await _phar.CreatePharmacie(pharmacie);
        //verify that the pharmacie was created successfully (optional)
        var createdPharmacie = await _phar.GetPharmacieById(_id);
        if (createdPharmacie == null)
        {
            throw new Exception("Failed to create pharmacie.");
        }
        
        return new CreatePharmacieResult(pharmacie);
        // Handle the command
    }
}
