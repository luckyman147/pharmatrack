using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.user.RefreshToken;

public record RefreshTokenCommand(string AccessToken, string RefreshToken) : ICommand<RefreshTokenResult>;

public record RefreshTokenResult(string AccessToken, string RefreshToken, DateTime AccessTokenExpiry);
