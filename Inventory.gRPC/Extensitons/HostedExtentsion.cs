
using Inventory.gRPC.Entities;
using MongoDB.Driver;
using Shared.Configurations.Database;
using Shared.Enums.Inventory;

namespace Inventory.gRPC.Extensitons
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

            SeedDataAsync(mongoClient, settings)
                .Wait();

            return host;
        }

        public static async Task SeedDataAsync(IMongoClient mongoClient, MongoSettings settings)
        {
            var databaseName = settings.DatabaseName;
            var database = mongoClient.GetDatabase(databaseName);
            var inventoryCollection = database.GetCollection<InventoryEntry>("Inventory");

            if (await inventoryCollection.EstimatedDocumentCountAsync() == 0)
            {
                await inventoryCollection.InsertManyAsync(getPreconfiguredInventoryEntries());
            }
        }

        private static IEnumerable<InventoryEntry> getPreconfiguredInventoryEntries()
        {
            return new List<InventoryEntry>
            {
                new()
                {
                    Quantity = 10,
                    DocumentNo = Guid.NewGuid().ToString(),
                    ItemNo = "Lotus",
                    ExternalDocumentNo = Guid.NewGuid().ToString(),
                    DocumentType = EDocumentType.Purchase,
                },
                new()
                {
                    Quantity = 10,
                    DocumentNo = Guid.NewGuid().ToString(),
                    ItemNo = "Cadillac",
                    ExternalDocumentNo = Guid.NewGuid().ToString(),
                    DocumentType = EDocumentType.Purchase,
                }
            };
        }
    }
}
