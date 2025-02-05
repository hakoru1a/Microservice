using Constracts.Saga.OrderManager;
using Microsoft.AspNetCore.Mvc;
using Saga.Orchestrator.OrderManager;
using Saga.Orchestrator.Services.Interfaces;
using Shared.DTOs.Basket;
using System.ComponentModel.DataAnnotations;

namespace Saga.Orchestrator.Controllers
{
    [ApiController]
    [Route("")]
    public class CheckoutController : ControllerBase
    {
        //private ICheckoutService _checkoutService;
        private ISagaOrderManager<BasketCheckoutDto, OrderResponse> _sagaOrderManager; 
        public CheckoutController( ISagaOrderManager<BasketCheckoutDto, OrderResponse> sagaOrderManager)
        {
            //_checkoutService = checkoutService;
            _sagaOrderManager = sagaOrderManager;
        }

        [HttpPost]
        [Route(template: "{username}")]
        public async Task<IActionResult> CheckoutOrder([Required] string username,
                    [FromBody] BasketCheckoutDto model)
        {
            //var result = await _checkoutService.CheckoutOrder(username, model);
            model.UserName = username;
            var result = _sagaOrderManager.CreateOrder(model);
            return Accepted(result);
        }

    }
}
