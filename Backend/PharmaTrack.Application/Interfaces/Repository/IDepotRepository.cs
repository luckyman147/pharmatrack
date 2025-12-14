using System;
using PharmaTrack.Application.DTOs.depot;
using PharmaTrack.Application.DTOs.produit;
using PharmaTrack.Domain.Models.depot;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Application.Interfaces.Repository;

    public interface IDepotRepository
    {
        // --- CRUD ---
        Task AddAsync(Depot depot);
        Task UpdateAsync(Depot depot);
        Task DeleteAsync(Guid id);
        Task<Depot?> GetByIdAsync(Guid id);

        // --- Pagination + Filtering ---
        Task<PaginationResult<DepotSummaryInfo>> GetAllPaginatedAsync(
            DepotFilter? filter = null,
           PaginationRequest? pagination = null);

        // --- Product-related operations ---
        Task<PaginationResult<LigneDepotProduit>> GetProduitsByDepotAsync(
            Guid depotId,
            PaginationRequest? pagination = null);
        Task<int> countDepotsinProduits(Guid produitId);

        Task UpdateProduitStockAsync(Guid depotId, LigneDepotProduit ligneProduit);
        Task RemoveProduitFromDepotAsync(Guid depotId, Guid produitId);
        Task RemoveProductFromAllDepotsAsync(Guid produitId);

        Task<PaginationResult<DepotsProduitInfo>> GetDepotsByProduitAsync(
            Guid produitId,
            DepotFilter? filter = null,
            PaginationRequest? pagination = null    );

        // --- Unified Grouping ---
        /// <summary>
        /// Groups depots by a specified address field ("Zone", "Gouvernorat", or "Delegation")
        /// with optional filtering and pagination.
        /// </summary>
        Task<PaginationResult<DepotGroupResult>> GroupByAsync(
            string groupByField,
            DepotFilter? filter = null,
            PaginationRequest? pagination = null);
    }