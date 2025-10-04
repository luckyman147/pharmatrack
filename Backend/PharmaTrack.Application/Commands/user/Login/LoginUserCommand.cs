using System;
using FluentValidation;
using PharmaTrack.Application.DTOs;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.user.Login;

public record LoginUserCommand (LoginDto loginDto) : ICommand<LoginReult>;
public record LoginReult(string AccessToken, DateTime AccessTokenExpiresAt, string RefreshToken);

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.loginDto.Code).NotEmpty().MaximumLength(100);
        RuleFor(x => x.loginDto.Password).NotEmpty().MinimumLength(6).MaximumLength(100);
        //Could be null if coming from trusted source
        RuleFor(x => x.loginDto.IpAddress).MaximumLength(45);
        // // IPv6 max length
    }
}
