using System;
using PharmaTrack.Domain.Models;

namespace PharmaTrack.Application.Interfaces.Repository;

public interface ISessionRepository
{
    Task AddAsync(UserSession session);
    Task<UserSession?> GetByRefreshTokenHashAsync(string refreshTokenHash);
    Task RevokeAsync(Guid sessionId, string? replacedByTokenHash = null);
    Task<IEnumerable<UserSession>> GetActiveSessionsForUser(Guid userId);
    Task RemoveExpiredAsync();
}
