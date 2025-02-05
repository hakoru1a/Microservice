using AutoMapper;
using Constracts.Common.Interface;
using Constracts.Indentity;
using Infrastructure.Common;
using Infrastructure.Common.Repository;
using Infrastructure.Extensions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Product.API.Persistence;
using Product.API.Repository;
using Product.API.Repository.Interface;
using Shared.Configurations;
using Shared.Configurations.Database;
using System.Text;
namespace Product.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();
        services.ConfigureProductDbContext(configuration);
        services.AddInfrastructureServices();
        services.ConfigureSwagger();
        services.AddConfigurationSettings(configuration);
        services.AddJwtAuthentication();
        services.ConfigureHealthCheck();
        return services;
    }
    private static IMapper AddMapper ()
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MapperProfile());
        });

        return mapperConfig.CreateMapper();
    }
    private static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

        var databaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();

        services.AddSingleton(jwtSettings);

        services.AddSingleton(databaseSettings);

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

    private static void ConfigureHealthCheck(this IServiceCollection services)
    {
        var databaseSettings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));

        if (string.IsNullOrWhiteSpace(databaseSettings?.ConnectionStrings))
        {
            throw new InvalidOperationException("The database connection string is missing or empty. Please verify your configuration.");
        }

        services.AddHealthChecks()
                .AddMySql(
                    connectionString: databaseSettings.ConnectionStrings,
                    name: "MySql Health",
                    failureStatus: HealthStatus.Degraded);
    }

    private static IServiceCollection ConfigureProductDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(name: "DefaultConnectionString");
        
        var builder = new MySqlConnectionStringBuilder(connectionString);

        services.AddDbContext<ProductContext>(optionsAction: m => m.UseMySql(builder.ConnectionString,
            ServerVersion.AutoDetect(builder.ConnectionString), mySqlOptionsAction: e =>
            {
                e.MigrationsAssembly("Product.API");
                e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
            }));

        return services;
    }
    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        return services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>))
                      .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                      .AddScoped<IProductRepository, ProductRepository>()
                      .AddSingleton(AddMapper())
                      .AddTransient<ITokenService, TokenService>();
    }
    private static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Ecom API",
                Version = "v1",
                Description = "E-commerce API for Product Management",  // Added meaningful description
                Contact = new OpenApiContact
                {
                    Name = "Chuong Dang",
                    Email = "hakoru1a@gmail.com",
                }
            });
        });
    }
}