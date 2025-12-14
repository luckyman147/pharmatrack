using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PharmaTrack.Application.Data;
using PharmaTrack.Domain.Models;
using PharmaTrack.Domain.Models.Commande;
using PharmaTrack.Domain.Models.depot;
using PharmaTrack.Domain.Models.Pharamcie;
using PharmaTrack.Domain.Models.Produit;
using PharmaTrack.Domain.Models.Visite;

using PharmaTrack.Domain.Models.Shared;

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
    public IMongoCollection<Pharmacie> Pharmacies => _database.GetCollection<Pharmacie>("Pharmacies");
    public IMongoCollection<Visite> Visites => _database.GetCollection<Visite>("Visites");
    public IMongoCollection<Categorie> Categories =>_database.GetCollection<Categorie>("Categories");
    public IMongoCollection<Produit> Produits =>_database.GetCollection<Produit>("Produits");
    public IMongoCollection<Depot> Depots => _database.GetCollection<Depot>("Depots");
    public IMongoCollection<Commande> Commandes => _database.GetCollection<Commande>("Commandes");
    public IMongoCollection<StockTransactionHistory> StockTransactionHistories => _database.GetCollection<StockTransactionHistory>("StockTransactionHistories");
    public IMongoCollection<ActivityLog> ActivityLogs => _database.GetCollection<ActivityLog>("ActivityLogs");

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // In Mongo, most operations are applied immediately.
        // SaveChanges can be used for transactions if needed.
        return Task.CompletedTask;
    }
}