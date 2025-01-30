using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Saga.Orchestrator.HttpRepository;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Saga.Orchestrator.Services;
using Saga.Orchestrator.Services.Interfaces;

namespace Saga.Orchestrator.Extensions
{
    public static class ServiceExtenstion
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddEndpointsApiExplorer();
            services.ConfigureSwagger();
            services.ConfigureHttpRepository();
            services.ConfigureHttpClients();
            services.ConfigureMapper();


            return services;
        }
        public static void ConfigureHttpRepository(this IServiceCollection services)
          => services.AddScoped<IOrderHttpRepository, OrderHttpRepository>()
                     .AddScoped<IBasketHttpRepository, BasketHttpRepository>()
                     .AddScoped<IInventoryHttpRepository, IventoryHttpRepository>()
                     .AddScoped<ICheckoutService, CheckoutService>();
        public static void ConfigureHttpClients(this IServiceCollection services)
        {
            ConfigureOrderHttpClient(services);
            ConfigureBasketHttpClient(services);
            ConfigureIventoryHttpClient(services);
        }

        private static void ConfigureMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(config =>
            {
                config.AddProfile(new MappingProfile());
            });
        }

        private static void ConfigureOrderHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<IOrderHttpRepository, OrderHttpRepository>(name: "OrdersAPI", configureClient: (sp, cl) =>
   {
                cl.BaseAddress = new Uri("http://localhost:5005/api/");
            });

            services.AddScoped(sp => sp.GetService<IHttpClientFactory>()
                .CreateClient(name: "OrdersAPI"));
        }
        private static void ConfigureBasketHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<IBasketHttpRepository, BasketHttpRepository>(name: "BasketAPI", configureClient: (sp, cl) =>
            {
                cl.BaseAddress = new Uri("http://localhost:5003/api/");
            });

            services.AddScoped(sp => sp.GetService<IHttpClientFactory>()
                .CreateClient(name: "BasketAPI"));
        }
        private static void ConfigureIventoryHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<IInventoryHttpRepository, IventoryHttpRepository>(name: "IventoryAPI", configureClient: (sp, cl) =>
            {
                cl.BaseAddress = new Uri("http://localhost:5006/api/");
            });

            services.AddScoped(sp => sp.GetService<IHttpClientFactory>()
                .CreateClient(name: "IventoryAPI"));
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
}
