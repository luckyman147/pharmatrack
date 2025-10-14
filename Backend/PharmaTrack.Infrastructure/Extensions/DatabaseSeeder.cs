using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using PharmaTrack.Application.Data;
using PharmaTrack.Domain.Models.user;
using PharmaTrack.Domain.ValueObject;
using System.Collections.Generic;
using PharmaTrack.Domain.Models;
using PharmaTrack.Domain.asset;
using PharmaTrack.Domain.Models.Pharamcie;
using System.Text.Json;
using System.Text.Json.Serialization;
using PharmaTrack.Domain.Models.Visite;

namespace PharmaTrack.Infrastructure.Extensions;

public static class DatabaseSeeder
{
    public static async Task InitialiseDatabaseAsync(this Microsoft.AspNetCore.Builder.WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        await SeedAsync(context);
        await SeedPharmaciesAsync(context);
        await SeedVisitesAsync(context);
    }

    private static async Task SeedVisitesAsync(IApplicationDbContext context)
    {
        var delegueId = "a0087472-51cc-4a68-b43e-46a0a8f0beda";
        var existingVisites = await context.Visites.Find(v => v.delegueteInfo.Id == Guid.Parse(delegueId)).AnyAsync();
        if (existingVisites) return;
        var visites = new List<Visite>();
var existingPharmacies = await context.Pharmacies.Find(_ => true).ToListAsync();
        if (existingPharmacies == null || existingPharmacies.Count == 0)
        {
            Console.WriteLine("No pharmacies found to associate with visites.");
            return;
        }
     
    for (int i = 1; i <= existingPharmacies.Count; i++)
        {
            var pharmacy = existingPharmacies[i - 1];
            var visite = new Visite
            {
                id = Guid.NewGuid(),
Description= $"Visite {i} Description",
                CreatedAt = DateTime.Now,
                VisitDate = DateTime.Now.AddDays(-i), // Recent past dates
                pharamcieBasicInfo = new PharamcieBasicInfo { Id = pharmacy.id, name = pharmacy.Name, address = pharmacy.Location.Gouvernerat },
                delegueteInfo = new ProfilebasicInfo
                {
                    Id = Guid.Parse(delegueId),
                    fullName = "Delegue 1",

                }
            };
            visites.Add(visite);
        }
        await context.Visites.InsertManyAsync(visites);
    }

    private static async Task SeedPharmaciesAsync(IApplicationDbContext context)
    {
        string jsonFileName = "pharmacies_data.json";
        string jsonFilePath = "C://Users//Iyed//Desktop//Medical_Solution//Backend//PharmaTrack.Infrastructure//Extensions//" + jsonFileName;
        if (!File.Exists(jsonFilePath))
        {
            Console.WriteLine($"Error: JSON file not found at {jsonFilePath}");
            return;
        }

        string jsonContent = await File.ReadAllTextAsync(jsonFilePath);
        var pharmacies = JsonSerializer.Deserialize<List<Pharmacie>>(jsonContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Handle potential casing issues
        });
        if (pharmacies == null || pharmacies.Count == 0)
        {
            Console.WriteLine("No pharmacy data found in the JSON file.");
            return;
        }
        foreach (var pharmacy in pharmacies)
        {
            pharmacy.id = new Guid();
            pharmacy.CreatedAt = DateTime.Now;
            pharmacy.LastModified = DateTime.Now;
            }

        Console.WriteLine($"Found {pharmacies.Count} records. Applying cleaning and inserting...");
        var existing = await context.Pharmacies.Find(_ => true).AnyAsync();
        if (existing) return;
        await context.Pharmacies.InsertManyAsync(pharmacies);
    }

    private static async Task SeedAsync(IApplicationDbContext context)
    {
        var existingUsers = await context.Users.Find(_ => true).AnyAsync();
        if (existingUsers) return;

        var users = new List<User>();
            var code = "U" + Guid.NewGuid().ToString("N")[..6].ToUpper();

        // --- Admin ---
        var admin = new Admin(
            id: Guid.NewGuid(),
            code: code,
            fullName: "System Administrator",
            email: "admin@pharmatrack.com",
            passwordHash: BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            permissionsLevel: "Super"
        );
        admin.SetProfile(new UserProfile("HQ Office", AdministrativeData.GetRandomAddress(), "", ""));
        users.Add(admin);

        // --- Delegues (4) ---
        for (int i = 1; i <= 4; i++)
        {
            var codes = "U" + Guid.NewGuid().ToString("N")[..6].ToUpper();

            var delegue = new Delegue(
                id: Guid.NewGuid(),
                code: codes.ToString(),
                fullName: $"Delegue {i}",
                isActive:true,
                email: $"delegue{i}@pharmatrack.com",
                passwordHash: BCrypt.Net.BCrypt.HashPassword("Delegue123!"),
                salary: 1500 + i * 100 // optional default salary
            );
            delegue.SetProfile(new UserProfile($"Street {i}", AdministrativeData.GetRandomAddress(), "", ""));
            users.Add(delegue);
        }

        await context.Users.InsertManyAsync(users);
    }
}
