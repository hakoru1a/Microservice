using Constracts.Indentity;
using Infrastructure.Extensions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;
using Shared.Configurations;
using System.Text;
using MMLib.SwaggerForOcelot;
namespace Ocelot.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();
        services.ConfigureSwagger();
        services.ConfigureOcelot(configuration);
        services.ConfigureCors(configuration);
        services.AddConfigurationSettings(configuration);
        services.AddJwtAuthentication();
        services.AddTransient<ITokenService, TokenService>();
        return services;
    }

    public static void ConfigureOcelot(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOcelot(configuration)
            .AddPolly();
        services.AddSwaggerForOcelot(configuration, x =>
        {
            x.GenerateDocsForGatewayItSelf = false;
        });
    }
    private static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration["AllowOrigins"];

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => {
                builder.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
            });
        });
    }

    private static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        services.AddSingleton(jwtSettings);
        return services;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {

        var settings = services.GetOptions<JwtSettings>(nameof(JwtSettings));

        if (settings == null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));

        var tokenValidationParameter = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = false
        };

        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(x =>
        {
            x.SaveToken = true;
            x.RequireHttpsMetadata = false;
            x.TokenValidationParameters = tokenValidationParameter;

        });

        return services;
    }
    private static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Ecom API",
                Version = "v1",
                Description = "E-commerce API for Customer Management",  // Added meaningful description
                Contact = new OpenApiContact
                {
                    Name = "Chuong Dang",
                    Email = "hakoru1a@gmail.com",
                }
            });
        });
    }
}