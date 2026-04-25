using FluentValidation;
using Location.Tracking.Application.Shared;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Threading.RateLimiting;

namespace Location.Tracking.Api.Middleware
{
    public static class RateLimitMiddleware 
    {
        public static IServiceCollection AddApiRateLimiter (this IServiceCollection services, ApiLimitSettings apiLimitSettings)
        {
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("FixedLimiter", opt =>
                {
                    opt.AutoReplenishment = apiLimitSettings.AutoReplenishemnt;
                    opt.Window = TimeSpan.FromSeconds(apiLimitSettings.Window);
                    opt.PermitLimit = apiLimitSettings.PermitLimit;
                    opt.QueueLimit = apiLimitSettings.QueueLimit;
                });
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });

            return services;
        }
    }
}
