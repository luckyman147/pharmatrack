


using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PharmaTrack.Shared.Behaviours;

namespace PharmaTrack.Application;

    public static class DependencyInjectionApplication
    {
        public static IServiceCollection AddApplication(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddMediatR(ctg =>
            {
                ctg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                ctg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                ctg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            });
            return services;
        }

    
}