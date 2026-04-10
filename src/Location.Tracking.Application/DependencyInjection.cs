using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
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


            // Add other application services here later

            // Riok.Mapperly:
            // services.AddSingleton<IMapper, Mapper>();

            return services;
        }
    }
}
