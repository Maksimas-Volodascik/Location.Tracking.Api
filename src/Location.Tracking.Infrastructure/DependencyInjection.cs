using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Interfaces.Services;
using Location.Tracking.Application.Services;
using Location.Tracking.Infrastructure.Data;
using Location.Tracking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TrackingDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            //Repositories
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            //Services
            services.AddScoped<IUserService, UserService>();

            // Register future repositories:
            // services.AddScoped<ILocationRepository, LocationRepository>();
            // services.AddScoped<ITrackingSessionRepository, TrackingSessionRepository>();

            // Add other infrastructure services here (caching, external APIs, email, etc.)

            return services;
        }
    }
}
