using Location.Tracking.Application;
using Location.Tracking.Application.Shared;
using Location.Tracking.Infrastructure;
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

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtConfiguration")); //map Jwt configuration from appsettings.json to JwtSettings

builder.Services.AddRouting(ops =>
{
    ops.LowercaseUrls = true; //lowercase url
});

builder.Services.AddAuthentication();
   
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/docs");
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
