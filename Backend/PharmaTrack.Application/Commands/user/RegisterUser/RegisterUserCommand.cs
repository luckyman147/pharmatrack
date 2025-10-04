using System;
using FluentValidation;
using PharmaTrack.Application.DTOs;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.user.RegisterUser;

public record RegisterUserCommand(RegisterUserDto user):ICommand<RegisterUserResult>;
    public record RegisterUserResult(String Code);
public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.user.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.user.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.user.Password).NotEmpty().MinimumLength(6).MaximumLength(100).Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

    }
}