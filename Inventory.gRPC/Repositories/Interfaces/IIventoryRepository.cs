using Constracts.Common.Interface;
using Infrastructure.Common.Repository;
using Inventory.gRPC.Entities;

namespace Inventory.gRPC.Repositories.Interfaces
{
    public interface IIventoryRepository : IMongoDBRepository<InventoryEntry>
    {
        Task<int> GetStockEntity(string itemNo);
    }
}
