using Basket.API.Entities;
using Basket.API.Repository.Interface;
using Constracts.Common.Interface;
using Microsoft.Extensions.Caching.Distributed;
using ILogger = Serilog.ILogger;
namespace Basket.API.Repository
{
    public class BasketReository : IBasketReository
    {
        private IDistributedCache _redisCache;

        private ISerializeService _serializeService;

        private ILogger _logger; 

        public BasketReository(IDistributedCache cache, ISerializeService serializeService, ILogger logger)
        {
            _redisCache = cache;
            _serializeService = serializeService;
            _logger = logger;
        }
        public async Task<bool> DeleteBasket(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            try
            {
                await _redisCache.RemoveAsync(username);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error($"Delete cache for ${username} fail");
                return false;
            }
        }

        public async Task DeleteBasketFromUserName(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            await _redisCache.RemoveAsync(username);
        }

        public async Task<Cart?> GetBasketByUsername(string username)
        {
            var basket = await _redisCache.GetStringAsync(username);
            return !string.IsNullOrEmpty(basket) ? _serializeService.Deserialize<Cart>(basket) : null;
        }

        public async Task<Cart?> UpdateBasket(Cart cart, DistributedCacheEntryOptions options)
        {
            if (options != null)
            {
                await _redisCache.SetStringAsync(cart.Username,_serializeService.Serialize(cart),options);
            }
            else
            {
                await _redisCache.SetStringAsync(cart.Username, _serializeService.Serialize(cart));
            }

            var value =  await GetBasketByUsername(cart.Username);

            return value;
        }
    }
}
