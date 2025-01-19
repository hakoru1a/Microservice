using EventBus.Messages.IntergrationEvent.Interface;
using Infrastructure.Configurations;
using Infrastructure.Extensions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Order.API.Application.IntergateEvents.EventsHandler;
using Shared.Configurations;

namespace Ordering.API.Extensions;

public static class ServiceExtensions
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Config();
        services.ConfigureMassTransit();
        return services;
    }


    private static void ConfigureMassTransit(this IServiceCollection services)
    {
        var settings = services.GetOptions<EventBusSettings>(sectionName: nameof(EventBusSettings));
        if (settings == null || string.IsNullOrEmpty(settings.HostAddress))
            throw new ArgumentNullException(paramName: "EventBusSettings is not configured.");

        var mqConnection = new Uri(settings.HostAddress);
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(config =>
        {
            config.AddConsumersFromNamespaceContaining<BasketCheckoutHandler>();
            config.UsingRabbitMq(configure: (ctx, cfg) =>
            {
                cfg.Host(mqConnection);
                //cfg.ReceiveEndpoint("basket-checkout-queue", e =>
                //{
                //    e.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
                //});
                cfg.ConfigureEndpoints(ctx);
            });
            config.AddRequestClient<IBasketCheckoutEvent>();
        });
    }
    private static void Config(this IServiceCollection services)
    {
        var emailSettings = services.GetOptions<SMTPEmailSettings>(sectionName: nameof(SMTPEmailSettings));
        var eventBusSettings = services.GetOptions<EventBusSettings>(sectionName: nameof(EventBusSettings));

        services.AddSingleton(emailSettings);
        services.AddSingleton(eventBusSettings);

    }
}