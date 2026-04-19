using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Tracking.Infrastructure.Data;
using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Interfaces.Services;
using Location.Tracking.Application.Services;
using Location.Tracking.Infrastructure.Repositories;

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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IDeviceModelRepository, DeviceModelRepository>();

            //Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<IDeviceModelService, DeviceModelService>();


            // Add other infrastructure services here (caching, external APIs, email, etc.)

            return services;
        }
    }
}
