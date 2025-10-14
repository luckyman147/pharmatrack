using System;
using MongoDB.Bson;
using MongoDB.Driver;
using PharmaTrack.Application.Data;
using PharmaTrack.Application.DTOs.Visite;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models.Visite;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Infrastructure.Repositories;

public class VisiteRepository: IVisiteRepository
{
    private readonly IMongoCollection<Visite> _context;

    public VisiteRepository(IApplicationDbContext context)
    {
        _context = context.Visites;
    }

    public async Task CreateVisite(Visite visite)
    {
        await _context.InsertOneAsync(visite);
    }

    public async Task DeleteVisite(Guid id)
    {
        await _context.FindOneAndDeleteAsync   (v => v.id == id);
    }

    public Task<Visite> Get(Guid id)
    {
        return _context.Find(v => v.id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PaginationResult<object>>> GetFiltredVisites()
    {   
        throw new NotImplementedException();
     }


    

    public async Task<PaginationResult<Visite>> GetVisitesByPharmacieId(Guid pharmacieId, PaginationRequest request)
    {
        var filter = Builders<Visite>.Filter.Eq(v => v.pharamcieBasicInfo.Id, pharmacieId);
        var visites = await _context.Find(filter).Skip(request.PageIndex * request.PageSize)
                                   .Limit(request.PageSize)
                                   .ToListAsync();
        var totalCount = await _context.CountDocumentsAsync(filter);
        return new PaginationResult<Visite>
        (
           request.PageIndex,
           request.PageSize,
           totalCount,
           (int)Math.Ceiling((double)totalCount / request.PageSize),
              visites
        );
    }

    public async Task UpdateVisite(Visite visite)
    {
        await _context.ReplaceOneAsync(v => v.id == visite.id, visite);
    }

public async Task<PaginationResult<object>> GetVisitesByDeleguesId(GetVisiteByDelegueDto dto)
{
    // 1️⃣ Build filters
    var filter = BuildCombinedFilter(dto);

    // 2️⃣ Query total count (for pagination)
    var totalCount = await _context.CountDocumentsAsync(filter);

    // 3️⃣ Pagination parameters
    var pageIndex = dto?.PageIndex ?? 0;
    var pageSize = dto?.PageSize ?? 10;

    // 4️⃣ Fetch filtered + paginated results
    var visites = await _context
        .Find(filter)
        .SortByDescending(v => v.VisitDate)
        .Skip(pageIndex * pageSize)
        .Limit(pageSize)
        .ToListAsync();

    // 5️⃣ Group by date in memory
    var groupedResults = visites
        .GroupBy(v => v.VisitDate.Date)
        .Select(g => new GroupedResult
        {
            Title = g.Key.ToString("dd MMMM yyyy"),
            Count = g.Count(),
            Items = g.ToList()
        })
        .ToList();

    // 6️⃣ Return paginated response
    return new PaginationResult<object>(
        pageIndex,
        pageSize,
        (int)totalCount,
        (int)Math.Ceiling((double)totalCount / pageSize),
        groupedResults
    );
}



    // --- Extracted Helper Methods ---

    // Builds the initial filter (Section 1 in your original code)
    private FilterDefinition<Visite> BuildCombinedFilter(GetVisiteByDelegueDto dto)
    {
        var baseFilter = Builders<Visite>.Filter.Eq(v => v.delegueteInfo.Id, dto.DelegueId);
        var dateFilters = new List<FilterDefinition<Visite>>();
       

        if (dto.startDate.HasValue)
        {
            dateFilters.Add(Builders<Visite>.Filter.Gte(v=>v.VisitDate, dto.startDate));
        }
        if (dto.endDate.HasValue)
        {
            var endOfDay = dto.endDate.Value.Date.AddDays(1).AddMilliseconds(-1);
            dateFilters.Add(Builders<Visite>.Filter.Lte(k=>k.VisitDate, endOfDay));
        }

        return dateFilters.Any() ? Builders<Visite>.Filter.And(baseFilter, Builders<Visite>.Filter.And(dateFilters)) : baseFilter;
    }

    // Creates the pipeline stages for Scenario 1 (Simple Filter)
private IEnumerable<BsonDocument> CreateDateGroupingPipeline()
{
    // Step 1: Sort visits by VisitDate descending (latest first)
    yield return new BsonDocument("$sort", new BsonDocument
    {
        { "VisitDate", -1 }
    });

    // Step 2: Group by VisitDate (formatted as yyyy-MM-dd)
    yield return new BsonDocument("$group", new BsonDocument
    {
        {
            "_id",
            new BsonDocument("$dateToString",
                new BsonDocument
                {
                    { "format", "%Y %B %d" },
                    { "date", "$VisitDate" },                    { "timezone", "UTC" }
                })
        },
        // The Title will just be the date string (e.g., "2025-10-12")
        { "Title", new BsonDocument("$first", new BsonDocument("$dateToString", new BsonDocument
            {
                { "format", "%d %B %Y " },
                { "date", "$VisitDate" },                    { "timezone", "UTC" }
            }))
        },
        { "Items", new BsonDocument("$push", "$$ROOT") },
        { "SortDate", new BsonDocument("$max", "$VisitDate") },
        { "Count", new BsonDocument("$sum", 1) }
    });

    // Step 3: Sort the grouped results by Title descending (latest date first)
    yield return new BsonDocument("$sort", new BsonDocument
    {
        { "SortDate", -1 }
    });

    // Step 4: Final projection
    yield return new BsonDocument("$project", new BsonDocument
    {
        { "_id", 0 },
        { "Title", "$Title" },
        { "Count", "$Count" },
        { "Items", "$Items" }
    });
}



}
