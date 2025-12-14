using System.Security.Claims;
using Microsoft.Extensions.Options;
using PharmaTrack.Application.Interfaces;
using PharmaTrack.Application.Interfaces.services;
using PharmaTrack.Application.configuration;
using PharmaTrack.Shared.CQRS;
using Microsoft.IdentityModel.JsonWebTokens;

namespace PharmaTrack.Application.Commands.user.RefreshToken;

public class RefreshTokenCommandHandler(
    IUserRepository userRepository,
    IJwtService jwtService,
    IOptions<JwtOptions> jwtOptions) 
    : ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private readonly JwtOptions _options = jwtOptions.Value;

    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = jwtService.ValidateAccessToken(request.AccessToken, validateLifetime: false);
        if (principal == null)
            throw new UnauthorizedAccessException("Invalid access token");

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier) 
                          ?? principal.FindFirst("sub") 
                          ?? principal.FindFirst(JwtRegisteredClaimNames.Sub);

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            throw new UnauthorizedAccessException("Invalid token claims");

        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new UnauthorizedAccessException("User not found");

        // Verify Refresh Token
        var incomingTokenHash = jwtService.HashToken(request.RefreshToken);

        if (user.RefreshTokenHash != incomingTokenHash || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token");
        }

        // Generate new tokens
        var newAccessToken = jwtService.GenerateAccessToken(user.id, user.Code, user.Role, user.Email);
        var newRefreshToken = jwtService.GenerateRefreshToken();
        
        user.UpdateRefreshToken(jwtService.HashToken(newRefreshToken), DateTime.UtcNow.AddDays(7));
        await userRepository.UpdateAsync(user);

        return new RefreshTokenResult(
            newAccessToken, 
            newRefreshToken, 
            DateTime.UtcNow.AddMinutes(_options.AccessTokenMinutes));
    }
}
