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
using PharmaTrack.Domain.Models.Produit;
using PharmaTrack.Domain.Converters;
using PharmaTrack.Domain.Models.depot;
using PharmaTrack.Domain.Models.shared;
using PharmaTrack.Domain.Models.Commande;
using PharmaTrack.Domain.enums;

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
        await SeedCategories(context);
        await SeedProduitsAsync(context);
        await SeedDepotsAsync(context);
        await SeedCommandesAsync(context);
    }
    
private static async Task SeedDepotsAsync(IApplicationDbContext context)
{
    var existingDepots = await context.Depots.Find(_ => true).AnyAsync();
    if (existingDepots) return;

    var products = await context.Produits.Find(_ => true).ToListAsync();
    if (products == null || products.Count == 0)
    {
        Console.WriteLine("No products found to assign to depots.");
        return;
    }

    var depots = new List<Depot>();

    for (int i = 1; i <= 10; i++)
    {
        var depot = new Depot
        {
            id = Guid.NewGuid(),
            Name = $"Depot {i}",
            Telephone = $"123-456-78{i:D2}",
            RapportDocumentUrl = "",
            Location = new Adress
            {
                Zone = $"Zone {i}",
                Gouvernerat = $"Gouvernorat {i}",
                delegation= $"Delegation {i}"
            },
            ProduitStock = new List<LigneDepotProduit>()
        };

        // Assign all products with quantity 7
        foreach (var prod in products)
        {
            depot.ProduitStock.Add(new LigneDepotProduit
            {
                ProduitId = prod.id,
                ProduitNom = prod.Name,
                Quantity = 7
            });
        }

        depots.Add(depot);
    }

    await context.Depots.InsertManyAsync(depots);
    Console.WriteLine("Depots seeded successfully with products.");
}








    private static async Task SeedProduitsAsync(IApplicationDbContext context)
    {
        var existingProduits = await context.Produits.Find(_ => true).AnyAsync();
        if (existingProduits) return;
        var categories = await context.Categories.Find(_ => true).ToListAsync();
        if (categories == null || categories.Count == 0)
        {
            Console.WriteLine("No categories found to associate with produits.");
            return;
        }
        var produits = new List<Produit>
        {
            new Produit { id = Guid.NewGuid(), Name = "Paracetamol", Description = "Pain reliever and fever reducer", PrixVente = (double)5.0m, Categorie = categories.Find(c => c.name == "Analgesics")!.ToCategoryInfo() },
            new Produit { id = Guid.NewGuid(), Name = "Amoxicillin", Description = "Antibiotic for bacterial infections", PrixVente = (double)10.0m, Categorie = categories.Find(c => c.name == "Antibiotics")!.ToCategoryInfo() },
            new Produit { id = Guid.NewGuid(), Name = "Oseltamivir", Description = "Antiviral medication for flu", PrixVente = (double)15.0m, Categorie = categories.Find(c => c.name == "Antivirals")!.ToCategoryInfo() },
            new Produit { id = Guid.NewGuid(), Name = "Fluconazole", Description = "Antifungal medication", PrixVente = (double)12.0m, Categorie = categories.Find(c => c.name == "Antifungals")!.ToCategoryInfo() },
            new Produit { id = Guid.NewGuid(), Name = "Ibuprofen", Description = "Anti-inflammatory drug", PrixVente = (double)8.0m, Categorie = categories.Find(c => c.name == "Anti-inflammatories")!.ToCategoryInfo() },

            new Produit { id = Guid.NewGuid(), Name = "Loratadine", Description = "Antihistamine for allergy relief", PrixVente = (double)7.0m, Categorie = categories.Find(c => c.name == "Antihistamines")! .ToCategoryInfo() },
            new Produit { id = Guid.NewGuid(), Name = "Sertraline", Description =
    "Antidepressant medication", PrixVente = (double)20.0m, Categorie = categories.Find(c => c.name == "Antidepressants")! .ToCategoryInfo() },
            new Produit { id = Guid.NewGuid(), Name = "Risperidone", Description = "Antipsychotic drug", PrixVente = (double)25.0m, Categorie = categories.Find(c => c.name == "Antipsychotics")! .ToCategoryInfo() },
            new Produit { id = Guid.NewGuid(), Name = "Lisinopril", Description = "Antihypertensive medication", PrixVente = (double)18.0m, Categorie = categories.Find(c => c.name == "Antihypertensives")! .ToCategoryInfo() },
            // Add more produits as needed
            
        };
        await context.Produits.InsertManyAsync(produits);

        Console.WriteLine("Produits seeded successfully.");
    }

    private static async Task SeedCategories(IApplicationDbContext context)
    {
        var existingCategories = await context.Categories.Find(_ => true).AnyAsync();
        if (existingCategories) return;
        var categories = new List<Categorie>
        {
            new Categorie { id = Guid.NewGuid(), name = "Analgesics" },
            new Categorie { id = Guid.NewGuid(), name = "Antibiotics" },
            new Categorie { id = Guid.NewGuid(), name = "Antivirals" },
            new Categorie { id = Guid.NewGuid(), name = "Antifungals" },
            new Categorie { id = Guid.NewGuid(), name = "Anti-inflammatories" },
            new Categorie { id = Guid.NewGuid(), name = "Antihistamines" },
            new Categorie { id = Guid.NewGuid(), name = "Antidepressants" },
            new Categorie { id = Guid.NewGuid(), name = "Antipsychotics" },
            new Categorie { id = Guid.NewGuid(), name = "Antihypertensives" },
            new Categorie { id = Guid.NewGuid(), name = "Diuretics" },
            new Categorie { id = Guid.NewGuid(), name = "Antidiabetics" },
            new Categorie { id = Guid.NewGuid(), name = "Cardiovascular" },
            new Categorie { id = Guid.NewGuid(), name = "Respiratory" },
            new Categorie { id = Guid.NewGuid(), name = "Gastrointestinal" },
            new Categorie { id = Guid.NewGuid(), name = "Dermatologicals" },
            new Categorie { id = Guid.NewGuid(), name = "Ophthalmic" },
            new Categorie { id = Guid.NewGuid(), name = "Vaccines" },
            new Categorie { id = Guid.NewGuid(), name = "Hormones & Endocrine" },
            new Categorie { id = Guid.NewGuid(), name = "Vitamins & Supplements" },
            new Categorie { id = Guid.NewGuid(), name = "Oncology" },
            new Categorie { id = Guid.NewGuid(), name = "Immunosuppressants" }
        };

        await context.Categories.InsertManyAsync(categories);
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
string basePath = AppContext.BaseDirectory;
string jsonFilePath = Path.Combine(basePath, "Extensions", jsonFileName);
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
                commission: 7,
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

    private static async Task SeedCommandesAsync(IApplicationDbContext context)
    {
        var existingCommandes = await context.Commandes.Find(_ => true).AnyAsync();
        if (existingCommandes) return;

        // Get necessary data
        var pharmacies = await context.Pharmacies.Find(_ => true).ToListAsync();
        var products = await context.Produits.Find(_ => true).ToListAsync();
        var depots = await context.Depots.Find(_ => true).ToListAsync();
        var users = await context.Users.Find(_ => true).ToListAsync();

        if (pharmacies == null || pharmacies.Count == 0 ||
            products == null || products.Count == 0 ||
            depots == null || depots.Count == 0 ||
            users == null || users.Count == 0)
        {
            Console.WriteLine("Missing required data for seeding commandes.");
            return;
        }

        var commandes = new List<Commande>();
        var random = new Random();
        var statuses = new[] { StatusCommande.EnAttente, StatusCommande.Valide, StatusCommande.EnRetour, StatusCommande.    Annulee };

        // Create 15 sample commandes
        for (int i = 1; i <= 15; i++)
        {
            var pharmacy = pharmacies[random.Next(pharmacies.Count)];
            var user = users[random.Next(users.Count)];
            var  status = statuses[0];
            Console.WriteLine($"Status: {status}");

            var commande = new Commande
            {
                id = Guid.NewGuid(),
                PharamcieInfo = new PharamcieBasicInfo
                {
                    Id = pharmacy.id,
                    name = pharmacy.Name,
                    address = pharmacy.Location.Gouvernerat,
                 
                },
                UserInfo = new ProfilebasicInfo
                {
                    Id = user.id,
                    fullName = user.FullName
                },
                OrderDate = DateTime.Now.AddDays(-random.Next(1, 90)),
                Status = StatusCommande.EnAttente,
                CodeComm = 1000 + i,
                OriginalDocumentPath = "",
                ExportedDocumentPath = "",
                Lines = new List<LigneCommande>()
            };

            // Add 2-5 order lines per commande
            int lineCount = random.Next(2, 6);
            for (int j = 0; j < lineCount; j++)
            {
                var product = products[random.Next(products.Count)];
                var line = new LigneCommande
                {
                    ProduitId = product.id,
                    NomProuit = product.Name,
                    UniteProduitPrix = product.PrixVente,
                    allocationsDepots = new List<AllocationDepot>()
                };

                // Add 1-3 depot allocations per line
                int depotCount = random.Next(1, 4);
                var selectedDepots = depots.OrderBy(x => random.Next()).Take(depotCount).ToList();

                foreach (var depot in selectedDepots)
                {
                    line.allocationsDepots.Add(new AllocationDepot
                    {
                       DepotInfo= new DepotBasicInfo {
                           Id = depot.id,
                           Name = depot.Name,
                           address = depot.Location
                       },
                       QuantityAllocated = random.Next(1, 11)
                    });
                }

                commande.Lines.Add(line);
            }

            commandes.Add(commande);
        }

        await context.Commandes.InsertManyAsync(commandes);
        Console.WriteLine($"Seeded {commandes.Count} commandes successfully.");
    }
}
