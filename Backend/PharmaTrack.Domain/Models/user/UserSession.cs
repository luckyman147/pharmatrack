using System;
using PharmaTrack.Domain.Abstractions;

namespace PharmaTrack.Domain.Models;

public class UserSession : Aggregate<Guid>
{
    public Guid UserId { get; set; }
    public string RefreshTokenHash { get; set; } // SHA256 hash of token

    public DateTime ExpiresAt { get; set; }
    public  string CreatedByIp { get; set; }
    public bool Revoked { get; set; } = false;
    public DateTime? RevokedAt { get; set; }
    public  string ReplacedByTokenHash { get; set; } // rotation link
    public bool IsActive => !Revoked && DateTime.UtcNow < ExpiresAt;
    public UserSession() { }
    public UserSession(Guid id, Guid userId, string refreshTokenHash, DateTime expiresAt, string createdByIp)
    {
        this.id = id;
        UserId = userId;
        RefreshTokenHash = refreshTokenHash;
        ExpiresAt = expiresAt;
        CreatedByIp = createdByIp;
    }
    public void Revoke(string revokedByIp, string replacedByTokenHash = "")
    {
        Revoked = true;
        RevokedAt = DateTime.UtcNow;
        ReplacedByTokenHash = replacedByTokenHash;
        CreatedByIp = revokedByIp;
    }


}
