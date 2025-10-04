using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using PharmaTrack.Application.Data;
using PharmaTrack.Domain.Models;
using PharmaTrack.Domain.Models.user;
using PharmaTrack.Domain.ValueObject;

namespace PharmaTrack.Infrastructure.Extensions;


public static class DatabaseSeeder
{
    public static async Task InitialiseDatabaseAsync(this Microsoft.AspNetCore.Builder.WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        await SeedAsync(context);
    }

    private static async Task SeedAsync(IApplicationDbContext context)
    {
        var existingUsers = await context.Users.Find(_ => true).AnyAsync();
        if (existingUsers) return;

        var users = new List<User>();

        // Admin
        var admin = new User(

            Guid.NewGuid(),
            "ADMIN001",
            "System Administrator",
            "admin@pharmatrack.com",
            BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            "Admin"
        );
        admin.SetProfile(new UserProfile("HQ Office", "+21690000000" ,"",""));
        users.Add(admin);

        // Delegates
        for (int i = 1; i <= 20; i++)
        {
            var   code = 10000000 + i; // 10000001, 10000002, ...

            var user = new User(
                Guid.NewGuid(),
                code.ToString(),
                $"Delegate {i}",
                $"delegate{i}@pharmatrack.com",
                BCrypt.Net.BCrypt.HashPassword("Delegate123!"),
                "Delegue"
            );
            user.SetProfile(new UserProfile($"Street {i}", $"+21690000{i:D3}","",""));
            users.Add(user);
        }

        await context.Users.InsertManyAsync(users);
    }
}
