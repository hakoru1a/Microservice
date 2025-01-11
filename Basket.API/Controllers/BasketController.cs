using Basket.API.Entities;
using Basket.API.Repository.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController: ControllerBase
    {
        private IBasketReository _repository;
        public BasketController(IBasketReository repository)
        {
            _repository = repository;
        }

        [HttpGet("{username}", Name = "GetBasket")]
        public async Task<ActionResult<Cart>> GetBasketByUsername([Required] string username)
        {
            var result = await _repository.GetBasetKetByUsername(username);
            return Ok(result ?? new Cart());
        }

        [HttpPost(Name = "UpdateBasket")]
        public async Task<ActionResult<Cart>> UpdateBasket([FromBody] Cart cart)
        {
            //var options = new DistributedCacheEntryOptions()
            //    .SetAbsoluteExpiration(DateTime.UtcNow.AddHours(1))
            //    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            var result = await _repository.UpdateBasket(cart, null);
            return Ok(result);
        }

        [HttpDelete(template: "{username}", Name = "DeleteBasket")]
        public async Task<ActionResult<bool>> DeleteBasket([Required] string username)
        {
            var result = await _repository.DeleteBasket(username);
            return Ok(result);
        }
    }
}
