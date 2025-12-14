using MongoDB.Driver;
using PharmaTrack.Application.Data;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models.Commande;

namespace PharmaTrack.Infrastructure.Repositories;

public class CommandeRepository : ICommandeRepository
{
    private readonly IMongoCollection<Commande> _commandes;

    public CommandeRepository(IApplicationDbContext context)
    {
        _commandes = context.Commandes;
    }

    public async Task AddAsync(Commande commande)
    {
        await _commandes.InsertOneAsync(commande);
    }

    public async Task UpdateAsync(Commande commande)
    {
        await _commandes.ReplaceOneAsync(c => c.id == commande.id, commande);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _commandes.DeleteOneAsync(c => c.id == id);
    }

    public async Task<Commande?> GetByIdAsync(Guid id)
    {
        return await _commandes.Find(c => c.id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Commande>> GetAllAsync()
    {
        return await _commandes.Find(_ => true).ToListAsync();
    }

    public async Task<IEnumerable<Commande>> GetByPharmacieIdAsync(Guid pharmacieId)
    {
        return await _commandes.Find(c => c.PharamcieInfo.Id == pharmacieId).ToListAsync();
    }

    public async Task<Commande?> GetByCodeCommAsync(int codeComm)
    {
        return await _commandes.Find(c => c.CodeComm == codeComm).FirstOrDefaultAsync();
    }

    public async Task RemoveProductFromAllCommandesAsync(Guid produitId)
    {
        var update = Builders<Commande>.Update.PullFilter(c => c.Lines, l => l.ProduitId == produitId);
        await _commandes.UpdateManyAsync(Builders<Commande>.Filter.Empty, update);
    }
}
