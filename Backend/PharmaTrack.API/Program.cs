using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PharmaTrack.API;
using PharmaTrack.Infrastructure.Configuration;
using PharmaTrack.Application;
using PharmaTrack.Infrastructure.Extensions;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration).AddApi(builder.Configuration).AddApplication(builder.Configuration);
// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(

)
;

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".PharmaTrack.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
        options.Cookie.SameSite = SameSiteMode.Strict; // CSRF protection
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});


builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.InitialiseDatabaseAsync().Wait();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSession();
app.UseApiServices();
app.UseAuthentication();
app.MapControllers();

app.UseAuthorization();
app.UseHttpsRedirection();

app.Run();
