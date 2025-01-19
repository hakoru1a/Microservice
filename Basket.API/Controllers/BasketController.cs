using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repository;
using Basket.API.Repository.Interface;
using EventBus.Messages.IntergrationEvent.Event;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController: ControllerBase
    {
        private IBasketReository _repository;

        private IMapper _mapper;

        private IPublishEndpoint _publishEndpoint;

        public BasketController(IBasketReository repository, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("{username}", Name = "GetBasket")]
        public async Task<ActionResult<Cart>> GetBasketByUsername([Required] string username)
        {
            var result = await _repository.GetBasketByUsername(username);
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

        [Route(template: "[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _repository.GetBasketByUsername(basketCheckout.UserName);
            if (basket == null) return NotFound();

            // publish checkout event to EventBus Message
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);

            eventMessage.TotalPrice = basket.TotalPrice;

            // publish the event using MassTransit
            await _publishEndpoint.Publish(eventMessage);

            // remove the basket
            await _repository.DeleteBasketFromUserName(basketCheckout.UserName);

            return Accepted();
        }
    }
}
