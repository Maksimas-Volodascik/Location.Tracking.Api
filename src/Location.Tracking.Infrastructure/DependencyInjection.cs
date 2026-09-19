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
using Location.Tracking.Infrastructure.Repositories;
using Location.Tracking.Application.Auth;
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

            //Repositories
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IDeviceModelRepository, DeviceModelRepository>();
            services.AddScoped<IRecordRepository, RecordRepository>();
            services.AddScoped<ILogEntryRepository, LogEntryRepository>();

            services.AddSingleton<ITokenIssuer, TokenIssuer>();
            // Add other infrastructure services here (caching, external APIs, email, etc.)

            return services;
        }
    }
}
