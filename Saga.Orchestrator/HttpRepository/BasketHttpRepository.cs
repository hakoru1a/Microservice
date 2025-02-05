using Saga.Orchestrator.HttpRepository.Interfaces;
using Shared.DTOs.Basket;

namespace Saga.Orchestrator.HttpRepository
{
    public class BasketHttpRepository : IBasketHttpRepository
    {
        public readonly HttpClient _client;

        public BasketHttpRepository(HttpClient client)
        {
            _client = client;
        }

        public Task<bool> DeleteBasket(string username)
        {
            return Task.FromResult(false);
        }

        public async Task<CartDto> GetBasket(string username)
        {
            var cart = await _client.GetFromJsonAsync<CartDto>($"basket/{username}");
            if (cart == null || !cart.Items.Any()) return null;
            return cart;
        }
    }
}
