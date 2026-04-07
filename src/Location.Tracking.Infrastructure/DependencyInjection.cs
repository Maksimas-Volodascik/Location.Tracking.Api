using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Tracking.Infrastructure.Data;

namespace Location.Tracking.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<TrackingDbContext>(options =>
               // options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));


            // Register future repositories:
            // services.AddScoped<ILocationRepository, LocationRepository>();
            // services.AddScoped<ITrackingSessionRepository, TrackingSessionRepository>();

            // Add other infrastructure services here (caching, external APIs, email, etc.)

            return services;
        }
    }
}
