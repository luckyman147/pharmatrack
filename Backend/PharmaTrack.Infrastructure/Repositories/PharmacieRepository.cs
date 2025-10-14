using System;
using System.Formats.Asn1;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using PharmaTrack.Application.Data;
using PharmaTrack.Application.DTOs.Pharamcie;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.enums;
using PharmaTrack.Domain.Models.Pharamcie;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Infrastructure.Repositories;

public class PharmacieRepository : IPharamcieRepository

{
    private readonly IMongoCollection<Pharmacie> _pharmacies;
    public PharmacieRepository(IApplicationDbContext context)
    {
        _pharmacies = context.Pharmacies;
        
    }
          public async Task CreatePharmacie(Pharmacie pharmacie)
        {
            await _pharmacies.InsertOneAsync(pharmacie);
        }

        // ✅ Update pharmacie by replacing the whole document
        public async Task Updateharmacie(Pharmacie pharmacie)
        {
            await _pharmacies.ReplaceOneAsync(p => p.id == pharmacie.id, pharmacie);
        }

    // ✅ Update pharmacie by ID (for example, toggle active state or similar)
    public async Task DeletePharmacie(Guid id)
    {
        var pharmacie = await _pharmacies.FindOneAndDeleteAsync(p => p.id == id);

        if (pharmacie == null)
        {
            // Log or handle the case where the pharmacy was not found
            Console.WriteLine($"Pharmacie with id {id} not found or already deleted.");
            return;
        }
    }
public async Task<PaginationResult<object>> GetPharmaciesAsync(PharmacieFilter filter, string? groupBy = null)
{
    var (finalFilter, filteredFields) = BuildPharmacieFilter(filter);

    if (!string.IsNullOrEmpty(groupBy) && !filteredFields.Contains(groupBy.ToLower()))
    {
        return await GetGroupedPharmaciesAsync(filter, finalFilter,filteredFields, groupBy);
    }

    // Default: regular pagination
    return await GetPaginatedPharmaciesAsync(filter, finalFilter);
}
private (FilterDefinition<Pharmacie> filter, HashSet<string> filteredFields) BuildPharmacieFilter(PharmacieFilter filter)
{
    var builder = Builders<Pharmacie>.Filter;
    var filters = new List<FilterDefinition<Pharmacie>>();
    var filteredFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    if (!string.IsNullOrEmpty(filter.Type) &&
        Enum.TryParse<PharmacieType>(filter.Type, true, out var typeEnum))
    {
        filters.Add(builder.Eq(p => p.PharmacieType, typeEnum));
        filteredFields.Add("pharmacietype");
    }

    if (!string.IsNullOrEmpty(filter.Horaire) &&
        Enum.TryParse<Horaire>(filter.Horaire, true, out var horaireEnum))
    {
        filters.Add(builder.Eq(p => p.Horaire, horaireEnum));
        filteredFields.Add("horaire");
    }

    if (!string.IsNullOrEmpty(filter.Zone))
    {
        filters.Add(builder.Eq(p => p.Location.Zone, filter.Zone));
        filteredFields.Add("zone");
    }

    if (!string.IsNullOrEmpty(filter.Gouvernerat))
    {
        filters.Add(builder.Eq(p => p.Location.Gouvernerat, filter.Gouvernerat));
        filteredFields.Add("gouvernerat");
    }

    if (!string.IsNullOrEmpty(filter.Delegation))
    {
        filters.Add(builder.Eq(p => p.Location.delegation, filter.Delegation));
        filteredFields.Add("delegation");
    }

    var finalFilter = filters.Count > 0 ? builder.And(filters) : builder.Empty;
Console.WriteLine("Filtered Fields: " + string.Join(", ", filteredFields));
    return (finalFilter, filteredFields);
}
private async Task<PaginationResult<object>> GetGroupedPharmaciesAsync(
    PharmacieFilter filter,
    FilterDefinition<Pharmacie> finalFilter,
    HashSet<string> values,
    string groupBy)
{
    string groupField = groupBy.ToLower() switch
    {
        "zone" => "$Location.Zone",
        "delegation" => "$Location.delegation",
        "gouvernerat" => "$Location.Gouvernerat",
        "horaire" => "$Horaire",
        "pharmacietype" => "$Type",
        _ => throw new ArgumentException($"Invalid groupBy field '{groupBy}'. Must be one of: zone, delegation, gouvernerat, horaire, pharmacietype.")
    };
    if (values .Contains(groupBy.ToLower()))
    {
        throw new ArgumentException("GroupBy cannot be one of the filters already applied.");
    }

    var pipeline = _pharmacies.Aggregate()
        .Match(finalFilter)
        .Group(new BsonDocument
        {
            { "_id", groupField },
            { "count", new BsonDocument("$sum", 1) },
            { "pharmacies", new BsonDocument("$push", "$$ROOT") }
        });

    var results = await pipeline.ToListAsync();

    var grouped = results.Select(r => new PharmacieGroupResult
    {
        Key = r["_id"].IsBsonNull ? "Inconnu" : r["_id"].ToString(),
        Count = r["count"].AsInt32,
        Pharmacies = r["pharmacies"].AsBsonArray
            .Select(b => MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Pharmacie>(b.AsBsonDocument))
            .ToList()
    }).ToList();

    var totalCount = grouped.Count;

    var pagedGroups = grouped
        .Skip(filter.PageIndex * filter.PageSize)
        .Take(filter.PageSize)
        .ToList();

    return new PaginationResult<object>(
        filter.PageIndex,
        filter.PageSize,
        totalCount,
        (int)Math.Ceiling((double)totalCount / (filter.PageSize)    ),
        pagedGroups
    );
}
private async Task<PaginationResult<object>> GetPaginatedPharmaciesAsync(
    PharmacieFilter filter,
    FilterDefinition<Pharmacie> finalFilter)
{
    var totalDocs = await _pharmacies.CountDocumentsAsync(finalFilter);

    var pharmacies = await _pharmacies
        .Find(finalFilter)
        .Skip(filter.PageIndex * filter.PageSize)
        .Limit(filter.PageSize)
        .ToListAsync();

    return new PaginationResult<object>(
        filter.PageIndex,
        filter.PageSize,
        totalDocs,
        (int)Math.Ceiling((double)totalDocs / filter.PageSize),
        pharmacies
    );
}

    // ✅ Filter pharmacies by optional criteria
  public async  Task<PaginationResult<Pharmacie>> GetFilteredPharmaciesAsync(PharmacieFilter filter)
        {
        Console.WriteLine("he re");
            var builder = Builders<Pharmacie>.Filter;
            var filters = new List<FilterDefinition<Pharmacie>>();

            
            if (!string.IsNullOrEmpty(filter.Type) &&
    Enum.TryParse<PharmacieType>(filter.Type, true, out var typeEnum))
{
    filters.Add(builder.Eq(p => p.PharmacieType, typeEnum));
}

if (!string.IsNullOrEmpty(filter.Horaire) &&
    Enum.TryParse<Horaire>(filter.Horaire, true, out var horaireEnum))
{
    filters.Add(builder.Eq(p => p.Horaire, horaireEnum));
}
            if (!string.IsNullOrEmpty(filter.Zone))
                filters.Add(builder.Eq(p => p.Location.Zone, filter.Zone));

            if (!string.IsNullOrEmpty(filter.Gouvernerat))
                filters.Add(builder.Eq(p => p.Location.Gouvernerat, filter.Gouvernerat));

            if (!string.IsNullOrEmpty(filter.Delegation))
                filters.Add(builder.Eq(p => p.Location.delegation, filter.Delegation));

        Console.WriteLine("he re");
            var finalFilter = filters.Count > 0 ? builder.And(filters) : builder.Empty;
        Console.WriteLine("he re");

           var totalCount = await _pharmacies.CountDocumentsAsync(finalFilter);

    // Apply pagination
        Console.WriteLine("he re");
    var data = await _pharmacies
        .Find(finalFilter)
        .Skip(filter.PageIndex * filter.PageSize)
        .Limit(filter.PageSize)
        .ToListAsync();

        return new PaginationResult<Pharmacie>(

          filter.PageIndex,
            filter.PageSize,
            totalCount,
          (int)Math.Round((double)totalCount / filter.PageSize),
        data
    );
        }

        // ✅ Search pharmacies by name (case-insensitive)
        public async Task<IEnumerable<Pharmacie>> GetPharmacieByName(string name)
        {
            var filter = Builders<Pharmacie>.Filter.Regex(
                p => p.Name,
                new BsonRegularExpression(name, "i") // case-insensitive search
            );

            return await _pharmacies.Find(filter).ToListAsync();
        }



    public async Task<IEnumerable<PharmacieGroupResult>> GroupPharmacieByAsync(string fieldName)
    {
       string groupField = fieldName.ToLower() switch
        {
            "zone" => "$Location.Zone",
            "delegation" => "$Location.delegation",
            "gouvernerat" => "$Location.Gouvernerat",
            "horaire" => "$Horaire",
            "pharmacietype" => "$Type",
            _ => throw new ArgumentException($"Invalid field name '{fieldName}'. Must be one of: zone, delegation, gouvernerat, horaire, pharmacieType.")
        };

        // Build aggregation pipeline
        var pipeline = _pharmacies.Aggregate()
            .Group(new BsonDocument
            {
                { "_id", groupField },
                { "count", new BsonDocument("$sum", 1) },
                { "pharmacies", new BsonDocument("$push", "$$ROOT") }
            });

        var results = await pipeline.ToListAsync();

        // Map BSON results to your domain model
        return results.Select(r => new PharmacieGroupResult
        {
            Key = r["_id"].IsBsonNull ? "Inconnu" : r["_id"].ToString(),
            Count = r["count"].AsInt32,
            Pharmacies = r["pharmacies"].AsBsonArray
                .Select(b => MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Pharmacie>(b.AsBsonDocument))
                .ToList()
        });
    }

    public async Task<Pharmacie?> GetPharmacieById(Guid id)
    {
        return await _pharmacies.Find(p => p.id == id).FirstOrDefaultAsync();
    }
}