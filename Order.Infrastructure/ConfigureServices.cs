using Constracts.Common.Interface;
using Constracts.Services;
using Infrastructure.Common;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Order.Infrastructure.Persistence;

namespace Order.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(name: "DefaultConnectionString");
            services.AddDbContext<OrderContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    builder => builder.MigrationsAssembly(typeof(OrderContext).Assembly.FullName))
            );
            services.AddScoped<OrderContextSeed>();
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped(typeof(ISMTPEmailServices), typeof(SMTTEmailServices));

            services.ConfigureSwagger();
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
}
