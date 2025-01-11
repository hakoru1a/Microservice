using Basket.API.Repository;
using Basket.API.Repository.Interface;
using Constracts.Common.Interface;
using Infrastructure.Common;
using Microsoft.OpenApi.Models;
namespace Basket.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();
        services.AddInfrastructureServices();
        services.ConfigureSwagger();
        services.AddRedis(configuration);
        return services;
    }

    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        return services.AddScoped<IBasketReository, BasketReository>()
                 .AddTransient<ISerializeService, SerializeService>();
    }
    private static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnectionString")
            ?? throw new ArgumentNullException(nameof(configuration), "Default connection string is not configured");
        services.AddStackExchangeRedisCache(services =>
        {
            services.Configuration = connectionString;
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