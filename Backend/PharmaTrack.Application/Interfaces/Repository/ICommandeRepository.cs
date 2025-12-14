using PharmaTrack.Domain.Models.Commande;


namespace PharmaTrack.Application.Interfaces.Repository;

public interface ICommandeRepository 
{
    Task<IEnumerable<Commande>> GetByPharmacieIdAsync(Guid pharmacieId);
    Task<Commande?> GetByCodeCommAsync(int codeComm);

    Task<Commande?> GetByIdAsync(Guid id);
    Task UpdateAsync(Commande commande);
    Task AddAsync(Commande commande);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Commande>> GetAllAsync();
    Task RemoveProductFromAllCommandesAsync(Guid produitId);
}
