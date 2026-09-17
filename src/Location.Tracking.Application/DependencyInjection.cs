using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Location.Tracking.Application.Auth;
using Location.Tracking.Application.AutoMapper;
using Location.Tracking.Application.Mapper;
using Location.Tracking.Application.Shared;
using Location.Tracking.Application.Shared.AuthToken;
using Location.Tracking.Application.Shared.Interface;
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
                cfg.AddProfile<UserProfile>();
            });

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Add other application services here later
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenIssuer, TokenIssuer>();

            return services;
        }
    }
}
