using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.enums;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.pharmacie.UpdatePharamcie;

public class UpdatePharmacieCommandHandler(IPharamcieRepository pharamcieRepository): ICommandHandler<UpdatePharmacieCommand, UpdatePharmacieResult>
{
    private readonly IPharamcieRepository _phar = pharamcieRepository;
    public async Task<UpdatePharmacieResult> Handle(UpdatePharmacieCommand request, CancellationToken cancellationToken)
    {
        var existingPharmacie = await _phar.GetPharmacieById(request.Id);
        if (existingPharmacie == null)
        {
            throw new KeyNotFoundException($"Pharmacie with ID {request.Id} not found.");
        }

        // Update the properties of the existing pharmacie with the values from the DTO
        existingPharmacie.Name = request.dto.Name ?? existingPharmacie.Name;
        existingPharmacie.Location = request.dto.Location?? existingPharmacie.Location;
        existingPharmacie.Telephone = request.dto.Telephone ?? existingPharmacie.Telephone;
        existingPharmacie.Location = request.dto.Location ?? existingPharmacie.Location;
     if (!string.IsNullOrEmpty(request.dto.Horaire) &&
                Enum.TryParse<Horaire>(request.dto.Horaire, true, out var parsedHoraire))
            {
                existingPharmacie.Horaire = parsedHoraire;
            }

            // ✅ Convert PharmacieType (string → enum)
            if (!string.IsNullOrEmpty(request.dto.PharmacieType) &&
                Enum.TryParse<PharmacieType>(request.dto.PharmacieType, true, out var parsedType))
            {
                existingPharmacie.PharmacieType = parsedType;
            }
        await _phar.Updateharmacie(existingPharmacie);

        return new UpdatePharmacieResult(existingPharmacie);
    }
}

