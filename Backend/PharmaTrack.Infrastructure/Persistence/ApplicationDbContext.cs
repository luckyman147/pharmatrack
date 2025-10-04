using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PharmaTrack.Application.Data;
using PharmaTrack.Domain.Models;

namespace PharmaTrack.Infrastructure.Persistence;

 public class ApplicationDbContext : IApplicationDbContext
    {
        private readonly IMongoDatabase _database;

        public ApplicationDbContext(IOptions<MongoDbSettings> settings)
        {
        var client = new MongoClient(settings.Value.MongoDbConnection);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<UserSession> UserSessions => _database.GetCollection<UserSession>("UserSessions");


        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // In Mongo, most operations are applied immediately.
        // SaveChanges can be used for transactions if needed.
        return Task.CompletedTask;
    }
    }