using System;
using PharmaTrack.Domain.Models.Produit;

namespace PharmaTrack.Application.Interfaces.Repository;

public interface ICategorieRepository
{
  Task CreateCategorieAsync(Categorie categorie);

        // --- Read ---
        Task<Categorie?> GetCategorieByIdAsync(Guid id);
        Task<IEnumerable<Categorie>> GetAllCategoriesAsync(string? name);
        Task<IEnumerable<Categorie>> GetCategoriesByNameAsync(string name);

        // --- Update ---
        Task UpdateCategorieAsync(Categorie categorie);

        // --- Delete ---
        Task DeleteCategorieAsync(Guid id);
}
