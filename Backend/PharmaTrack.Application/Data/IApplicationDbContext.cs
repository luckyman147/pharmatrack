using System;
using MongoDB.Driver;
using PharmaTrack.Domain.Models;
using PharmaTrack.Domain.Models.depot;
using PharmaTrack.Domain.Models.Pharamcie;
using PharmaTrack.Domain.Models.Produit;
using PharmaTrack.Domain.Models.Visite;
using PharmaTrack.Domain.Models.Commande;
using PharmaTrack.Domain.Models.Shared;

namespace PharmaTrack.Application.Data;

public interface IApplicationDbContext
{
    IMongoCollection<User> Users { get; }
    IMongoCollection<UserSession> UserSessions { get; }
    IMongoCollection<Pharmacie> Pharmacies { get; }
    IMongoCollection<Visite> Visites { get; }
    IMongoCollection<Produit> Produits { get; }
    IMongoCollection<Categorie> Categories { get; }
    IMongoCollection<Depot> Depots { get; }
    IMongoCollection<Commande> Commandes { get; }
    IMongoCollection<StockTransactionHistory> StockTransactionHistories { get; }
    IMongoCollection<ActivityLog> ActivityLogs { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}