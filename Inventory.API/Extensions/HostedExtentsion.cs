using Inventory.API.Persistence;
using MongoDB.Driver;
using Shared.Configurations.Database;

namespace Inventory.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var settings = services.GetService<MongoSettings>();

            if (settings == null || string.IsNullOrEmpty(settings.ConnectionStrings))
                throw new ArgumentNullException("DatabaseSettings is not configured.");

            var mongoClient = services.GetRequiredService<IMongoClient>();

            new InventorySeed()
                .SeedDataAsync(mongoClient, settings)
                .Wait();

            return host;
        }
    }
}
