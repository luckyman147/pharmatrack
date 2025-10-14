using FluentValidation;
using System.Linq;
using System;
using PharmaTrack.Domain.asset;
using PharmaTrack.Domain.Models.shared;


public class AdressValidator : AbstractValidator<Adress>
{
    public AdressValidator()
    {
        // 1. Rule for Zone: Ensure the provided Zone name exists in the hierarchy.
        RuleFor(a => a.Zone)
            .NotEmpty().WithMessage("La Zone administrative est requise.")
            .Must(zone => AdministrativeData.GetZoneData(zone) != null)
            .WithMessage(a => $"'{a.Zone}' n'est pas une Zone administrative valide (ex: Nord-Est, Sud).");

        // 2. Rule for Gouvernerat: Ensure the provided Gouvernerat exists within the selected Zone.
        RuleFor(a => a.Gouvernerat)
            .NotEmpty().WithMessage("Le Gouvernerat est requis.")
            .Must((a, gouvernerat) =>
            {
                // Get the Zone data record.
                var zoneData = AdministrativeData.GetZoneData(a.Zone);
                
                // If Zone is invalid, skip this specific check (it will fail on the Zone rule anyway).
                if (zoneData == null) return true; 

                // Check if the Gouvernerat exists within that specific Zone.
                return zoneData.Governorates.Any(g => g.Name.Equals(gouvernerat, StringComparison.OrdinalIgnoreCase));
            })
            .WithMessage(a => $"'{a.Gouvernerat}' n'est pas un Gouvernerat valide pour la Zone '{a.Zone}'.")
            // This rule only runs if the Zone validation passed, preventing cascading errors.
            .When(a => AdministrativeData.GetZoneData(a.Zone) != null);

        // 3. Rule for Delegation: Ensure the provided Delegation exists within the selected Gouvernerat.
        // NOTE: We use 'a.delegation' to match the property name in the Adress class.
        RuleFor(a => a.delegation) 
            .NotEmpty().WithMessage("La Délégation est requise.")
            .Must((a, delegation) =>
            {
                // Find the zone data.
                var zoneData = AdministrativeData.GetZoneData(a.Zone);
                if (zoneData == null) return true; 

                // Find the specific Gouvernerat within that zone.
                var governorateData = zoneData.Governorates
                    .FirstOrDefault(g => g.Name.Equals(a.Gouvernerat, StringComparison.OrdinalIgnoreCase));

                // If Gouvernerat is invalid, skip this check.
                if (governorateData == null) return true;

                // Check if the Delegation exists within that specific Gouvernerat.
                return governorateData.Delegations.Any(d => d.Equals(delegation, StringComparison.OrdinalIgnoreCase));
            })
            .WithMessage(a => $"'{a.delegation}' n'est pas une Délégation valide pour le Gouvernerat '{a.Gouvernerat}'.")
            // This rule only runs if both Zone and Gouvernerat validation checks passed successfully.
            .When(a => AdministrativeData.GetZoneData(a.Zone) != null && 
                       AdministrativeData.GetZoneData(a.Zone)?.Governorates
                           .Any(g => g.Name.Equals(a.Gouvernerat, StringComparison.OrdinalIgnoreCase)) == true);
    }
}