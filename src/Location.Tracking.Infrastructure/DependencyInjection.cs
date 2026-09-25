using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Location.Tracking.Infrastructure.Data;
using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Infrastructure.Auth;

namespace Location.Tracking.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TrackingDbContext>(options =>
                options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION") ?? configuration.GetConnectionString("DefaultConnection")));

            //Contexct
            services.AddScoped<ITrackingDbContext>(sp => sp.GetRequiredService<TrackingDbContext>());

            services.AddSingleton<ITokenIssuer, TokenIssuer>();
            // Add other infrastructure services here (caching, external APIs, email, etc.)

            return services;
        }
    }
}
