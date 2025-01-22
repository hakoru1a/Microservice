﻿using AutoMapper;
using Infrastructure.Extensions;
using Inventory.gRPC.Repositories;
using Inventory.gRPC.Repositories.Interfaces;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Shared.Configurations.Database;

namespace Inventory.gRPC.Extensitons
{
    public static class ServiceExtensiton
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddEndpointsApiExplorer();
            services.AddSingleton(CreateMapper());
            services.AddConfigurationSettings(configuration);
            services.ConfigureMongoDbClient();
            services.AddScoped<IIventoryRepository, InventoryRepository>();
            return services;
        }


        private static IMapper CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            return mapperConfig.CreateMapper();

        }
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
                                                                        IConfiguration configuration)
        {
            var databaseSettings = configuration.GetSection(nameof(MongoSettings))//IConfigurationSection
                .Get<MongoSettings>();
            services.AddSingleton(databaseSettings);
            return services;
        }

        private static string getMongoConnectionString(this IServiceCollection services)
        {
            var settings = services.GetOptions<MongoSettings>(nameof(MongoSettings));
            if (settings == null || string.IsNullOrEmpty(settings.ConnectionStrings))
                throw new ArgumentNullException("DatabaseSettings is not configured.");

            var databaseName = settings.DatabaseName;
            var mongoDbConnectionString = settings.ConnectionStrings + "/" + databaseName + "?authSource=admin";
            return mongoDbConnectionString;
        }

        public static void ConfigureMongoDbClient(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(
                new MongoClient(getMongoConnectionString(services)))
                .AddScoped(x => x.GetService<IMongoClient>().StartSession());
        }
    }
}
