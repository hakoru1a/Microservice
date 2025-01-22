using Constracts.Common.Interface;
using Inventory.API.Entities;
using Shared.DTOs.Inventory;
using Shared.SeedWork;

namespace Inventory.API.Services.Interfaces
{
    public interface IIventoryService : IMongoDBRepository<InventoryEntry>
    {
        Task<IEnumerable<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo);
        
        Task<PagedList<InventoryEntryDto>> GetPageByItemNoAsync(GetInventoryPagingQuery query);

        Task<InventoryEntryDto> GetByIdAsync(string id);

        Task<InventoryEntryDto> PurchaseProduct(string itemNo ,PurchaseProductDto purchaseProductDto);
    }
}
