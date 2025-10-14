using System;
using AutoMapper;
using FluentValidation;
using PharmaTrack.Domain.Models.user;

namespace PharmaTrack.Application.validators;

public class ProfileValidator : AbstractValidator<UserProfile>
{
   public ProfileValidator()
        {
        // FullName Validation
        RuleFor(pro => pro.adress).NotEmpty().SetValidator(new AdressValidator());
        
            // Telephone Validation
        RuleFor(profile => profile.PhoneNumber)
                .NotEmpty().WithMessage("Telephone number is required.")
                // Basic validation for 8 to 15 digits, with an optional leading '+'
                .Matches(@"^\+?\d{8,12}$").WithMessage("Telephone number format is invalid. It should contain 8 to 12 digits.");

            // ImageUrl Validation (Optional but must be a valid URL if provided)
            RuleFor(profile => profile.AvatarUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(profile => !string.IsNullOrEmpty(profile.AvatarUrl))
                .WithMessage("Image URL must be a valid absolute URI.");
        }

}
