using System;
using System.Security.Claims;

namespace PharmaTrack.Application.Interfaces.services;

public interface IJwtService
{
           string GenerateAccessToken(Guid userId, string code, string role, string email);
    string GenerateRefreshToken();
    public ClaimsPrincipal? ValidateAccessToken(string token, bool validateLifetime = true);
        string HashToken(string token);
}
