using AutoMapper;
using Basket.API.Repository;
using Basket.API.Repository.Interface;
using Constracts.Common.Interface;
using EventBus.Messages.IntergrationEvent.Interface;
using Infrastructure.Common;
using Infrastructure.Configurations;
using Infrastructure.Extensions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Shared.Configurations;
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
        services.AddAutoMapper(config =>
        {
            config.AddProfile(new MappingProfile());
        });
        services.ConfigureMassTransit();
        services.AddConfigurationSettings(configuration);
        return services;
    }

    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        return services.AddScoped<IBasketReository, BasketReository>()
                 .AddTransient<ISerializeService, SerializeService>();
    }
    private static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = services.GetOptions<CacheSettings>(nameof(CacheSettings)).ConnectionStrings
     ?? throw new ArgumentNullException(nameof(configuration), "Default connection string is not configured");
        services.AddStackExchangeRedisCache(services =>
        {
            services.Configuration = connectionString;
        });

        return services;
    }

    public static void ConfigureMassTransit(this IServiceCollection services)
    {
        var settings = services.GetOptions<EventBusSettings>(sectionName: nameof(EventBusSettings));
        if (settings == null || string.IsNullOrEmpty(settings.HostAddress))
            throw new ArgumentNullException(paramName: "EventBusSettings is not configured.");

        var mqConnection = new Uri(settings.HostAddress);
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq(configure: (ctx, cfg) =>
            {
                cfg.Host(mqConnection);
            });
            config.AddRequestClient<IBasketCheckoutEvent>();
        });
    }
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        var eventBusSettings = configuration.GetSection(key: nameof(EventBusSettings)) //IConfigurationSection
            .Get<EventBusSettings>();
        services.AddSingleton(eventBusSettings);

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