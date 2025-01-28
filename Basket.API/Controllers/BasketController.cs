using AutoMapper;
using Basket.API.Entities;
using Basket.API.gRPCServices;
using Basket.API.Repository;
using Basket.API.Repository.Interface;
using Basket.API.Services.Interfaces;
using EventBus.Messages.IntergrationEvent.Event;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Shared.DTOs.Basket;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Cart = Basket.API.Entities.Cart;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController: ControllerBase
    {
        private IBasketRepository _repository;

        private IMapper _mapper;

        private IPublishEndpoint _publishEndpoint;

        private StockItemGrpcService _stockItemGrpcService;

        private IEmailTemplateService _emailTemplateService;


        public BasketController(IBasketRepository repository, IMapper mapper, 
            IPublishEndpoint publishEndpoint, StockItemGrpcService stockItemGrpcService, IEmailTemplateService emailTemplateService)
        {
            _repository = repository;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _stockItemGrpcService = stockItemGrpcService;
            _emailTemplateService = emailTemplateService;
        }

        [HttpGet("{username}", Name = "GetBasket")]
        public async Task<ActionResult<CartItemDto>> GetBasketByUsername([Required] string username)
        {
            var result = await _repository.GetBasketByUsername(username);
            return Ok(result);
        }

        [HttpPost(Name = "UpdateBasket")]
        public async Task<ActionResult<CartItemDto>> UpdateBasket([FromBody] CartDto model)
        {
            //var options = new DistributedCacheEntryOptions()
            //    .SetAbsoluteExpiration(DateTime.UtcNow.AddHours(1))   
            //    .SetSlidingExpiration(TimeSpan.FromMinutes(5));



            foreach (var item in model.Items) {
                //var stock = await _stockItemGrpcService.GetStock(item.No);
                item.AvailableStock = 10;
            }

            var entity = _mapper.Map<Cart>(model);
            var updated = await _repository.UpdateBasket(entity, null);
            var result = _mapper.Map<CartDto>(updated);
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


        [HttpPost(template: "[action]", Name = "SendEmailReminder")]
        public ContentResult SendEmailReminder()
        {
            var emailTemplate = _emailTemplateService
                .GenerateReminderCheckoutOrderEmail("test");

            var result = new ContentResult
            {
                Content = emailTemplate,
                ContentType = "text/html"
            };

            return result;
        }



    }
}
