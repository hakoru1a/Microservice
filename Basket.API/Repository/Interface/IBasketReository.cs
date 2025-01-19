using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Repository.Interface
{
    public interface IBasketReository
    {
        Task<Cart?> GetBasketByUsername(string username);
        Task<Cart?> UpdateBasket(Cart basket, DistributedCacheEntryOptions? options);
        Task<bool> DeleteBasket(string username);

        Task DeleteBasketFromUserName(string username);
    }
}
