using System;
using MongoDB.Driver;
using PharmaTrack.Domain.Models;
using PharmaTrack.Domain.Models.Pharamcie;
using PharmaTrack.Domain.Models.Visite;

namespace PharmaTrack.Application.Data;

  public interface IApplicationDbContext
    {
        IMongoCollection<User> Users { get; }
        IMongoCollection<UserSession> UserSessions { get; }
        IMongoCollection<Pharmacie> Pharmacies { get; }
        IMongoCollection<Visite> Visites { get; }

      
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }