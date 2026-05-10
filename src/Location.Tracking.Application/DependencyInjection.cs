using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Location.Tracking.Application.Features.AutoMapper;
using Location.Tracking.Application.Shared;
using Microsoft.Extensions.DependencyInjection;


namespace Location.Tracking.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication (this IServiceCollection services)
        {
            // Register FluentValidation (scans all validators in the assembly)
            services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection), includeInternalTypes: true);

            //services.AddSingleton<IMapper, Mapper>();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<DeviceProfile>();
                cfg.AddProfile<DeviceModelProfile>();
            });

            // Add other application services here later

            return services;
        }
    }
}
