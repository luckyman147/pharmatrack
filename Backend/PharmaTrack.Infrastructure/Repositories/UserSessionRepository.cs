using System;
using MongoDB.Driver;
using PharmaTrack.Application.Data;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models;

namespace PharmaTrack.Infrastructure.Repositories;

public class UserSessionRepository : ISessionRepository
{
     private readonly IMongoCollection<UserSession> _col;
    public UserSessionRepository(IApplicationDbContext context)
    {
        _col = context.UserSessions;
    }
    public Task AddAsync(UserSession session) => _col.InsertOneAsync(session);

    public Task<UserSession?> GetByRefreshTokenHashAsync(string refreshTokenHash) =>
        _col.Find(s => s.RefreshTokenHash == refreshTokenHash).FirstOrDefaultAsync()
            

        ;

    public Task RevokeAsync(Guid sessionId, string? replacedByTokenHash = null) =>
        _col.UpdateOneAsync(s => s.id == sessionId,
            Builders<UserSession>.Update
               .Set(s => s.Revoked, true)
               .Set(s => s.RevokedAt, DateTime.UtcNow)
               .Set(s => s.ReplacedByTokenHash, replacedByTokenHash));

    public Task<IEnumerable<UserSession>> GetActiveSessionsForUser(Guid userId) =>
        _col.Find(s => s.UserId == userId && !s.Revoked && s.ExpiresAt > DateTime.UtcNow).ToListAsync().ContinueWith(t => (IEnumerable<UserSession>)t.Result);

    public Task RemoveExpiredAsync() => _col.DeleteManyAsync(s => s.ExpiresAt < DateTime.UtcNow);
}
