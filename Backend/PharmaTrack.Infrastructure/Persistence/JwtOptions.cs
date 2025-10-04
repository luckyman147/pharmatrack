using System;

namespace PharmaTrack.Infrastructure.Persistence;

public class JwtOptions
{
    public string Secret { get; set; } = string.Empty;   // Your signing key
    public string Issuer { get; set; } = string.Empty;   // API issuer
    public string Audience { get; set; } = string.Empty; // API audience
    public int AccessTokenMinutes { get; set; } = 30;    // Token lifetime in minutes
}