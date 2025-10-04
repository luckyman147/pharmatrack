using System;
using Carter;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PharmaTrack.Shared.Exceptions.Handler;

namespace PharmaTrack.API;

   public static class DependencyInjection
   {
       public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
       {
           services.AddCarter();

           services.AddExceptionHandler<CustomExceptionHandler>();

           return services;
       }


       public static WebApplication UseApiServices(this WebApplication app)
       {
           app.MapCarter();

           app.UseExceptionHandler(options =>
           { 
            
           });
         

           return app;
       }
   }