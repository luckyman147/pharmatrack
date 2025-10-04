using System;
using MongoDB.Driver;
using PharmaTrack.Domain.Models;

namespace PharmaTrack.Application.Data;

  public interface IApplicationDbContext
    {
        IMongoCollection<User> Users { get; }
        IMongoCollection<UserSession> UserSessions { get; }

      
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }