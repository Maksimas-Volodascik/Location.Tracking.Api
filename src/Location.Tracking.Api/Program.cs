using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Location.Tracking.Api.Middleware;
using Location.Tracking.Application;
using Location.Tracking.Application.Shared;
using Location.Tracking.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();


//JWT Auth
builder.Services.AddAuthentication()
    .AddJwtBearer("BearerAuth", jwtOptions =>
    {
        jwtOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtConfiguration:Issuer"],////Must match the ValidIssuer

            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtConfiguration:Audience"], //Must match the ValidAudience

            ValidateLifetime = true, //checks "exp"
            RequireExpirationTime = true, //Forces token to have expiration time

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfiguration:Token"]!)) //Verifies tokens signature using secret key
        };
    });

//map JWT and API Limit configuration from appsettings.json to settings class
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtConfiguration"));
builder.Services.Configure<ApiLimitSettings>(builder.Configuration.GetSection("ApiRateLimiter"));

builder.Services.AddRouting(ops =>
{
    ops.LowercaseUrls = true; //lowercase url
});

//API Versioning
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    opt.ReportApiVersions = true;
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddMvc()
.AddApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

ApiLimitSettings apiLimitSettings = new ApiLimitSettings();
builder.Configuration.GetSection("ApiRateLimiter").Bind(apiLimitSettings);
builder.Services.AddApiRateLimiter(apiLimitSettings); //pass settings from appsettings to api limiter method

builder.Services.AddOpenApi("v1");
builder.Services.AddOpenApi("v2");

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("docs", opt =>
    {//load separate documentation for different versions
        opt.AddDocument("v1", "API Version 1.0", "/openapi/v1.json", isDefault: true);
        opt.AddDocument("v2", "API Version 2.0", "/openapi/v2.json");
    });
}

app.UseRateLimiter();

app.MapControllers()
    .RequireRateLimiting("FixedLimiter"); //apply rate limiting to all controllers

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
