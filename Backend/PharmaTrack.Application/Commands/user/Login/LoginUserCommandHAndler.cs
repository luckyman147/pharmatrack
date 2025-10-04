using System;
using Microsoft.Extensions.Options;
using PharmaTrack.Application.Interfaces;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Application.Interfaces.services;
using PharmaTrack.Domain.Models;



using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.user.Login;

public class LoginUserCommandHAndler(
    IUserRepository userRepository,
    ISessionRepository sessionRepository,
    IPasswordHasher hasher,
    IJwtService jwtService
    ) : ICommandHandler<LoginUserCommand, LoginReult>
{
        private readonly IUserRepository _userRepository = userRepository;
    private readonly ISessionRepository _sessionRepository = sessionRepository;
    private readonly IPasswordHasher _hasher = hasher;
    private readonly IJwtService _jwtService = jwtService;

    public async Task<LoginReult>  Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
 var dto = request.loginDto;

        // 1️⃣ Fetch user by Code
        var user = await _userRepository.GetByCodeAsync(dto.Code);
        if (user == null)
            throw new Exception("Invalid credentials");

        // 2️⃣ Verify password
        if (!_hasher.VerifyPassword( dto.Password,user.PasswordHash))
            throw new Exception("Invalid credentials");

        // 3️⃣ Generate Access Token
        var accessToken = _jwtService.GenerateAccessToken(user.id, user.Code, user.Role, user.Email);
        var accessTokenExpires = DateTime.UtcNow.AddMinutes(160);

        // 4️⃣ Generate Refresh Token & Hash
        var refreshToken = _jwtService.GenerateRefreshToken();
        var refreshTokenHash = _jwtService.HashToken(refreshToken);

        // 5️⃣ Create UserSession
        var session = new UserSession(
            
            id: Guid.NewGuid(),
            userId: user.id,
            refreshTokenHash: refreshTokenHash,
            expiresAt: DateTime.UtcNow.AddDays(7), // Refresh token lifetime
            createdByIp: dto.IpAddress
        );

        await _sessionRepository.AddAsync(session);

        // 6️⃣ Return tokens
        return new LoginReult(accessToken, accessTokenExpires, refreshToken);
    }
}
