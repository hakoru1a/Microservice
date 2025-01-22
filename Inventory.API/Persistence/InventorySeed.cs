using Inventory.API.Entities;
using Inventory.API.Extensions;
using MongoDB.Driver;
using Shared.Configurations.Database;
using Shared.Enums.Inventory;

namespace Inventory.API.Persistence
{
    public class InventorySeed
    {
        public async Task SeedDataAsync(IMongoClient mongoClient, MongoSettings settings)
        {
            var databaseName = settings.DatabaseName;
            var database = mongoClient.GetDatabase(databaseName);
            var inventoryCollection = database.GetCollection<InventoryEntry>("Inventory");

            if (await inventoryCollection.EstimatedDocumentCountAsync() == 0)
            {
                await inventoryCollection.InsertManyAsync(getPreconfiguredInventoryEntries());
            }
        }

        private  IEnumerable<InventoryEntry> getPreconfiguredInventoryEntries()
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
