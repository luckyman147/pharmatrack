using System;
using AutoMapper;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using PharmaTrack.Application.Data;
using PharmaTrack.Application.Interfaces;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Application.Interfaces.services;
using PharmaTrack.Infrastructure.Persistence;
using PharmaTrack.Infrastructure.Repositories;
using PharmaTrack.Infrastructure.Services;

namespace PharmaTrack.Infrastructure.Configuration;

public static class DependencyInjection
{
 public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            services.Configure<MongoDbSettings>(configuration.GetSection("ConnectionStrings"));
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
 services.AddScoped<IUserRepository, UserRepository>();
services.Configure<JwtOptions>(
    configuration.GetSection("Jwt")
);
            services.AddSingleton<IJwtService, JwtService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISessionRepository, UserSessionRepository>();
            services.AddScoped<IPharamcieRepository, PharmacieRepository>();
            services.AddScoped<IVisiteRepository, VisiteRepository>();

            services.AddMemoryCache();
            services.AddScoped<ICacheService, MemoryCacheService>();

            // Mapster
            services.AddSingleton(TypeAdapterConfig.GlobalSettings);

            return services;
        }
}
