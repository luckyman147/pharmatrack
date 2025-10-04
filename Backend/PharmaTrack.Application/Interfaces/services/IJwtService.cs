using System;

namespace PharmaTrack.Application.Interfaces.services;

public interface IJwtService
{
           string GenerateAccessToken(Guid userId, string code, string role, string email);
    string GenerateRefreshToken();
    string HashToken(string token);
}
