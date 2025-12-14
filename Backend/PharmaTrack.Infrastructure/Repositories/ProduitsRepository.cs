using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using global::PharmaTrack.Domain.Models.Produit;
using global::PharmaTrack.Shared.Pagination;
using MongoDB.Bson;
using MongoDB.Driver;
using PharmaTrack.Application.Data;
using PharmaTrack.Application.DTOs.produit;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models.depot;

namespace PharmaTrack.Infrastructure.Repositories;

public class ProduitsRepository : IProduitRepository
{
    private readonly IDepotRepository _depotRepository;
    private readonly IMongoCollection<Produit> _produits;
    private readonly IMongoCollection<Depot> _depots;
    private readonly IMongoCollection<Categorie> _categories;

    public ProduitsRepository(IApplicationDbContext context, IDepotRepository depotRepository)
    {
        _produits = context.Produits;
        _depots = context.Depots;
        _categories = context.Categories;
        _depotRepository = depotRepository;
    }

    // --- Create ---
    public async Task CreateProduitAsync(Produit produit)
    {
        await _produits.InsertOneAsync(produit);
    }

    // --- Read ---
    public async Task<ProduitDetailsDto?> GetProduitByIdAsync(Guid id)
    {
 var produit = await _produits.Find(p => p.id == id).FirstOrDefaultAsync();
    if (produit == null) return null;

    // Find all depots containing that product
    var depots = await _depots
        .Find(d => d.ProduitStock.Any(ps => ps.ProduitId == id))
        .ToListAsync();

    var depotInfos = depots
        .Select(d =>
        {
            var ligne = d.ProduitStock.First(ps => ps.ProduitId == produit.id);
            return new DepotsProduitInfo
            {
                DepotId = d.id,
                DepotName = d.Name,
                Quantity = ligne.Quantity
            };
        })
        .ToList();

    return new ProduitDetailsDto
    {
        Id = produit.id,
        Name = produit.Name,
        Description = produit.Description,
        PrixVente = produit.PrixVente,
        Categorie = produit.Categorie!,
        Depots = depotInfos
    }; }

    public async Task<PaginationResult<Produit>> GetAllProduitsAsync(PaginationRequest pagination)
    {
        var total = await _produits.CountDocumentsAsync(FilterDefinition<Produit>.Empty);
        
        var pipeline = new[]
        {
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "Categories" },
                { "localField", "CategorieId" },
                { "foreignField", "_id" },
                { "as", "Categorie" }
            }),
            new BsonDocument("$unwind", new BsonDocument
            {
                { "path", "$Categorie" },
                { "preserveNullAndEmptyArrays", true }
            }),
            new BsonDocument("$skip", (pagination.PageIndex - 1) * pagination.PageSize),
            new BsonDocument("$limit", pagination.PageSize)
        };

        var items = await _produits.Aggregate<Produit>(pipeline).ToListAsync();

        return CreatePaginationResult(items, total, pagination.PageIndex, pagination.PageSize);
    }

    public async Task<PaginationResult<Produit>> GetProduitsByCategorieAsync(Guid categorieId, PaginationRequest pagination)
    {
        var filter = Builders<Produit>.Filter.Eq(p => p.Categorie.id, categorieId);
        var total = await _produits.CountDocumentsAsync(filter);
        var items = await _produits
            .Find(filter)
            .Skip((pagination.PageIndex - 1) * pagination.PageSize)
            .Limit(pagination.PageSize)
            .ToListAsync();

        return CreatePaginationResult(items, total, pagination.PageIndex, pagination.PageSize);
    }

    public async Task<PaginationResult<Produit>> SearchProduitsByNameAsync(string name, PaginationRequest pagination)
    {
        var filter = Builders<Produit>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));
        var total = await _produits.CountDocumentsAsync(filter);
        var items = await _produits
            .Find(filter)
            .Skip((pagination.PageIndex - 1) * pagination.PageSize)
            .Limit(pagination.PageSize)
            .ToListAsync();

        return CreatePaginationResult(items, total, pagination.PageIndex, pagination.PageSize);
    }

    // --- Update ---
    public async Task UpdateProduitAsync(Produit produit , Guid? categorieId = null)
    {
        if (categorieId.HasValue)
        {
            var categorie = await _categories.Find(c => c.id == categorieId.Value).FirstOrDefaultAsync();
            var update = Builders<Produit>.Update.Set(p => p.Categorie.id, categorieId.Value);
            if (categorie != null)
            {
                update = update.Set(p => p.Categorie.name, categorie.name);
            }

            var filter = Builders<Produit>.Filter.Eq(p => p.id, produit.id);
            await _produits.UpdateOneAsync(filter, update);
            
        }
        else
        {
            var filter = Builders<Produit>.Filter.Eq(p => p.id, produit.id);
            await _produits.ReplaceOneAsync(filter, produit);
        }
    }

    // --- Delete ---
    public async Task DeleteProduitAsync(Guid id)
    {
        await _produits.DeleteOneAsync(p => p.id == id);
    }

    // --- Filtering ---
public async Task<PaginationResult<Produit>> GetFilteredProduitsAsync(ProduitFilter filter, PaginationRequest pagination)
{
    // The repository needs an injected IDepotRepository dependency
    // private readonly IDepotRepository _depotRepository;

    var combinedFilter = BuildFilter(filter);

    // 1. Calculate the total count first (unchanged)
    var total = await _produits.CountDocumentsAsync(combinedFilter);

    // 2. Use Find() for simple filtering, skipping, and limiting (returns IFindFluent<Produit>)
    var findFluent = _produits.Find(combinedFilter)
        .Skip((pagination.PageIndex - 1) * pagination.PageSize)
        .Limit(pagination.PageSize);

    // 3. Execute the query to get the Produit objects directly
    // The driver automatically deserializes the results into Produit objects,
    // solving previous BSON/Guid issues.
    var items = await findFluent.ToListAsync();

    // 4. Loop through the products and use the IDepotRepository to count associated depots
    // This replaces the MongoDB Lookup/Project aggregation logic.
    foreach (var produit in items)
    {
        // Assuming IDepotRepository has a method to count depots by ProduitId (which is produit.id)
        produit.numberDepots = await _depotRepository.countDepotsinProduits(produit.id);
    }

    // 5. Return the paginated result
    return CreatePaginationResult(items, total, pagination.PageIndex, pagination.PageSize);
}
    // --- Count ---
    public async Task<long> CountAsync(ProduitFilter? filter = null)
    {
        var combinedFilter = filter != null ? BuildFilter(filter) : FilterDefinition<Produit>.Empty;
        return await _produits.CountDocumentsAsync(combinedFilter);
    }

    // --- Private helpers ---
    private FilterDefinition<Produit> BuildFilter(ProduitFilter filter)
    {
        var builder = Builders<Produit>.Filter;
        var filters = new List<FilterDefinition<Produit>>();

        if (!string.IsNullOrEmpty(filter.Name))
            filters.Add(builder.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(filter.Name, "i")));

        if (filter.CategorieId.HasValue)
            filters.Add(builder.Eq(p => p.Categorie.id, filter.CategorieId.Value));

        if (filter.MinPrixVente.HasValue)
            filters.Add(builder.Gte(p => p.PrixVente, filter.MinPrixVente.Value));

        if (filter.MaxPrixVente.HasValue)
            filters.Add(builder.Lte(p => p.PrixVente, filter.MaxPrixVente.Value));
        if (!string.IsNullOrEmpty(filter.CategorieName))
        {
            filters.Add(builder.Regex(p => p.Categorie.name, new MongoDB.Bson.BsonRegularExpression(filter.CategorieName, "i")));


        }

        return filters.Count > 0 ? builder.And(filters) : builder.Empty;
    }

    private PaginationResult<TEntity> CreatePaginationResult<TEntity>(
        IEnumerable<TEntity> items, long totalCount, int pageIndex, int pageSize
    ) where TEntity : class
    {
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        int pagesReste = Math.Max(totalPages - pageIndex, 0);

        return new PaginationResult<TEntity>(
            pageIndex: pageIndex,
            pageSize: pageSize,
            count: totalCount,
            data: items
        );
    }


    public async Task<PaginationResult<ProduitGroupResult>> GroupByAsync(string fieldName, ProduitFilter? filter = null, PaginationRequest? pagination = null)
    {
        var combinedFilter = filter != null ? BuildFilter(filter) : FilterDefinition<Produit>.Empty;
        var group = new BsonDocument
        {
            { "_id", $"${fieldName}" },
            { "Count", new BsonDocument("$sum", 1) }
        };
        var aggregation = _produits.Aggregate()
            .Match(combinedFilter)
            .Group<ProduitGroupResult>(group);
        var countResult = await aggregation.Count().FirstOrDefaultAsync();
        long total = countResult?.Count ?? 0;
        if (pagination != null)
        {
            aggregation = aggregation
                .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                .Limit(pagination.PageSize);
        }
        var items = await aggregation.ToListAsync();
        return CreatePaginationResult(items, total, pagination?.PageIndex ?? 1, pagination?.PageSize ?? (int)total);
    }

}
