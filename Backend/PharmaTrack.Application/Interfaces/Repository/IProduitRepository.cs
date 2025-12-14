using System;
using PharmaTrack.Application.DTOs.produit;
using PharmaTrack.Domain.Models.Produit;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Application.Interfaces.Repository;

public interface IProduitRepository
{

        // --- Create ---
        Task CreateProduitAsync(Produit produit);

        // --- Read ---
        Task<ProduitDetailsDto?> GetProduitByIdAsync(Guid id);

        // Paginated listing of all products
        Task<PaginationResult<Produit>> GetAllProduitsAsync(PaginationRequest pagination);

        // Paginated listing by category
        Task<PaginationResult<Produit>> GetProduitsByCategorieAsync(Guid categorieId, PaginationRequest pagination);

        // Paginated search by name
        Task<PaginationResult<Produit>> SearchProduitsByNameAsync(string name, PaginationRequest pagination);

        // --- Update ---
        Task UpdateProduitAsync(Produit produit,Guid? categorieId = null);

        // --- Delete ---
        Task DeleteProduitAsync(Guid id);

        // --- Filtering ---
        Task<PaginationResult<Produit>> GetFilteredProduitsAsync(ProduitFilter filter, PaginationRequest pagination);

        // --- Grouping (now paginated) ---
        Task<PaginationResult<ProduitGroupResult>> GroupByAsync(
            string fieldName,
            ProduitFilter? filter = null,
            PaginationRequest? pagination = null
        );

        // --- Count ---
        Task<long> CountAsync(ProduitFilter? filter = null);
}
