using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using PharmaTrack.Application.DTOs.depot;
using PharmaTrack.Domain.Models.depot;
using PharmaTrack.Shared.Pagination;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Infrastructure.Helper;
using PharmaTrack.Application.DTOs.produit;
using PharmaTrack.Application.Data;

namespace PharmaTrack.Infrastructure.Repositories
{
    public class DepotsRepository : IDepotRepository
    {
        private readonly IMongoCollection<Depot> _depots;

        public DepotsRepository(IApplicationDbContext context)
        {
            _depots = context.Depots;
        }

        // --- CRUD ---
        public async Task AddAsync(Depot depot)
        {
            await _depots.InsertOneAsync(depot);
        }

        public async Task UpdateAsync(Depot depot)
        {
            await _depots.ReplaceOneAsync(d => d.id == depot.id, depot);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _depots.DeleteOneAsync(d => d.id == id);
        }

        public async Task<Depot?> GetByIdAsync(Guid id)
        {
            return await _depots.Find(d => d.id == id).FirstOrDefaultAsync();
        }

        // --- Pagination + Filtering ---
        public async Task<PaginationResult<DepotSummaryInfo>> GetAllPaginatedAsync(DepotFilter? filter = null, PaginationRequest? pagination = null)
        {
            var combinedFilter = FilterHelper.BuildFilter<Depot>((builder, filters) =>
            {
                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.Name))
                        filters.Add(builder.Regex(d => d.Name, new BsonRegularExpression(filter.Name, "i")));

                    if (!string.IsNullOrEmpty(filter.Telephone))
                        filters.Add(builder.Regex(d => d.Telephone, new BsonRegularExpression(filter.Telephone, "i")));

                    if (!string.IsNullOrEmpty(filter.Gouvernorat))
                        filters.Add(builder.Regex(d => d.Location.Gouvernerat, new BsonRegularExpression(filter.Gouvernorat, "i")));

                    if (!string.IsNullOrEmpty(filter.Delegation))
                        filters.Add(builder.Regex(d => d.Location.delegation, new BsonRegularExpression(filter
.Delegation, "i")));
                    if (!string.IsNullOrEmpty(filter.Zone))
                        filters.Add(builder.Regex(d => d.Location.Zone, new BsonRegularExpression(filter.Zone, "i")));
                }
            });

            var total = await _depots.CountDocumentsAsync(combinedFilter);
            var pageIndex = pagination?.PageIndex ?? 1;
            var pageSize = pagination?.PageSize ?? 10;

            var items = await _depots.Find(combinedFilter)
                .Skip((pageIndex - 1) * pageSize)
                .Limit(pageSize)
                .Project(d => new DepotSummaryInfo
                {
                    Id = d.id,
            Name = d.Name,
            Telephone = d.Telephone,
            Location = d.Location,
            NumberOfProducts = d.ProduitStock.Count,
            NumberOfStockTotal = d.ProduitStock.Sum(p => p.Quantity),
            ProduitsEnRupture = d.ProduitStock.Count(p => p.Quantity == 0),
            LastModified = d.LastModified
        })
                .ToListAsync();

            return new PaginationResult<DepotSummaryInfo>(pageIndex, pageSize, total, items);
        }

        // --- Product-related operations ---
        public async Task<PaginationResult<LigneDepotProduit>> GetProduitsByDepotAsync(Guid depotId, PaginationRequest? pagination = null)
        {
            var depot = await _depots.Find(d => d.id == depotId).FirstOrDefaultAsync();
            var products = depot?.ProduitStock ?? new List<LigneDepotProduit>();

            var pageIndex = pagination?.PageIndex ?? 1;
            var pageSize = pagination?.PageSize ?? 10;

            var pagedProducts = products
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return new PaginationResult<LigneDepotProduit>(pageIndex, pageSize, products.Count, pagedProducts);
        }

        public async Task UpdateProduitStockAsync(Guid depotId, LigneDepotProduit ligneProduit)
        {
            var filter = Builders<Depot>.Filter.Eq(d => d.id, depotId) & Builders<Depot>.Filter.ElemMatch(d => d.ProduitStock, p => p.ProduitId == ligneProduit.ProduitId);
            var update = Builders<Depot>.Update.Set(d => d.ProduitStock[-1].Quantity, ligneProduit.Quantity);
            await _depots.UpdateOneAsync(filter, update);
        }

        public async Task RemoveProduitFromDepotAsync(Guid depotId, Guid produitId)
        {
            var update = Builders<Depot>.Update.PullFilter(d => d.ProduitStock, p => p.ProduitId == produitId);
            await _depots.UpdateOneAsync(d => d.id == depotId, update);
        }

        public async Task RemoveProductFromAllDepotsAsync(Guid produitId)
        {
            var update = Builders<Depot>.Update.PullFilter(d => d.ProduitStock, p => p.ProduitId == produitId);
            await _depots.UpdateManyAsync(Builders<Depot>.Filter.Empty, update);
        }

        public async Task<PaginationResult<DepotsProduitInfo>> GetDepotsByProduitAsync(Guid produitId, DepotFilter? filter = null, PaginationRequest? pagination = null)
        {
            var combinedFilter = FilterHelper.BuildFilter<Depot>((builder, filters) =>
            {
                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.Name))
                        filters.Add(builder.Regex(d => d.Name, new BsonRegularExpression(filter.Name, "i")));

                    if (!string.IsNullOrEmpty(filter.Telephone))
                        filters.Add(builder.Regex(d => d.Telephone, new BsonRegularExpression(filter.Telephone, "i")));
                }

                filters.Add(builder.ElemMatch(d => d.ProduitStock, p => p.ProduitId == produitId));
            });

            var depots = await _depots.Find(combinedFilter).ToListAsync();
            var depotInfos = new List<DepotsProduitInfo>();

            foreach (var depot in depots)
            {
                var produit = depot.ProduitStock.Find(p => p.ProduitId == produitId);
                if (produit != null)
                {
                    depotInfos.Add(new DepotsProduitInfo
                    {
                        DepotId = depot.id,
                        DepotName = depot.Name,
                        Quantity = produit.Quantity
                    });
                }
            }

            var pageIndex = pagination?.PageIndex ?? 1;
            var pageSize = pagination?.PageSize ?? 10;

            var pagedDepotInfos = depotInfos
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return new PaginationResult<DepotsProduitInfo>(pageIndex, pageSize,  depotInfos.Count,  pagedDepotInfos);
        }

        // --- Unified Grouping ---
        public async Task<PaginationResult<DepotGroupResult>> GroupByAsync(string groupByField, DepotFilter? filter = null, PaginationRequest? pagination = null)
        {
            var combinedFilter = FilterHelper.BuildFilter<Depot>((builder, filters) =>
            {
                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.Name))
                        filters.Add(builder.Regex(d => d.Name, new BsonRegularExpression(filter.Name, "i")));

                    if (!string.IsNullOrEmpty(filter.Telephone))
                        filters.Add(builder.Regex(d => d.Telephone, new BsonRegularExpression(filter.Telephone, "i")));

                    if (!string.IsNullOrEmpty(filter.Gouvernorat))
                        filters.Add(builder.Regex(d => d.Location.Gouvernerat, new BsonRegularExpression(filter.Gouvernorat, "i")));
                }
            });

            var pipeline = new List<BsonDocument>();

            // Match stage
            var serializerRegistry = MongoDB.Bson.Serialization.BsonSerializer.SerializerRegistry;
            var documentSerializer = serializerRegistry.GetSerializer<Depot>();
            var renderContext = new RenderArgs<Depot>(documentSerializer, serializerRegistry);
            pipeline.Add(new BsonDocument("$match", combinedFilter.Render(renderContext)));

            // Group stage
            pipeline.Add(new BsonDocument("$group", new BsonDocument
            {
                { "_id", $"$Location.{groupByField}" },
                { "Count", new BsonDocument("$sum", 1) }
            }));

            var skip = (pagination?.PageIndex ?? 1 - 1) * (pagination?.PageSize ?? 10);
            var limit = pagination?.PageSize ?? 10;

            pipeline.Add(new BsonDocument("$skip", skip));
            pipeline.Add(new BsonDocument("$limit", limit));

            var results = await _depots.Aggregate<DepotGroupResult>(pipeline).ToListAsync();
            var totalCount = results.Count;

            return new PaginationResult<DepotGroupResult>(pagination?.PageIndex ?? 1, pagination?.PageSize ?? 10, totalCount, results);
        }

        public Task<int> countDepotsinProduits(Guid produitId)
        {
            var filter = Builders<Depot>.Filter.ElemMatch(d => d.ProduitStock, p => p.ProduitId == produitId);
            return _depots.CountDocumentsAsync(filter).ContinueWith(t => (int)t.Result);
        }
    }
}
