using Infrastructure.Common.Repository;
using Inventory.gRPC.Entities;
using Inventory.gRPC.Repositories.Interfaces;
using MongoDB.Driver;
using Shared.Configurations.Database;

namespace Inventory.gRPC.Repositories
{
    public class InventoryRepository : MongoDBRepository<InventoryEntry>, IIventoryRepository
    {
        public InventoryRepository(IMongoClient mongoClient, MongoSettings settings) : base(mongoClient, settings)
        {
        }

        public async Task<int> GetStockEntity(string itemNo)
                => Collection.AsQueryable().Where(x => x.ItemNo.Equals(itemNo)).Sum(x => x.Quantity);
    }
}
