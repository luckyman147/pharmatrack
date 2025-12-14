using System;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;
using PharmaTrack.Application.Data;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models.Produit;
using PharmaTrack.Shared.Exceptions;

namespace PharmaTrack.Infrastructure.Repositories;

public class CategoryRepository : ICategorieRepository
{
    private readonly IMongoCollection<Categorie> _categories;
    public CategoryRepository(IApplicationDbContext context)
    {
        _categories = context.Categories;
    }
    public async Task CreateCategorieAsync(Categorie categorie)
    {
        await _categories.InsertOneAsync(categorie);
     
    }

    public async Task DeleteCategorieAsync(Guid id)
    {
    var categorie = await _categories.Find(c => c.id == id).FirstOrDefaultAsync();
    if (categorie == null) throw new NotFoundException("Category not found");
    await _categories.DeleteOneAsync(c => c.id == id);
    }

    public async Task<IEnumerable<Categorie>> GetAllCategoriesAsync(String?name)
    {
   
        return await _categories.Find(_ => true).ToListAsync();
    
    }

    public async Task<Categorie?> GetCategorieByIdAsync(Guid id)
    {
        var categorie = await _categories.Find(c => c.id == id).FirstOrDefaultAsync();
        if (categorie == null) throw new NotFoundException("Not Found");
        return categorie;

    }

    public async Task<IEnumerable<Categorie>> GetCategoriesByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return await _categories.Find(_ => true).ToListAsync();

        var filter = Builders<Categorie>.Filter.Regex(
            c => c.name,
            new MongoDB.Bson.BsonRegularExpression(name, "i")
        );

        return await _categories.Find(filter).ToListAsync();
    }

    public async Task UpdateCategorieAsync(Categorie categorie)
    {
        var result = await _categories.ReplaceOneAsync(c => c.id == categorie.id, categorie);
        if (result.MatchedCount == 0)
            throw new NotFoundException("Category not found");
    }
}
