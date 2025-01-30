using Microsoft.AspNetCore.Mvc;
using Saga.Orchestrator.Services.Interfaces;
using Shared.DTOs.Basket;
using System.ComponentModel.DataAnnotations;

namespace Saga.Orchestrator.Controllers
{
    [ApiController]
    [Route("")]
    public class CheckoutController : ControllerBase
    {
        private ICheckoutService _checkoutService;
        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost]
        [Route(template: "{username}")]
        public async Task<IActionResult> CheckoutOrder([Required] string username,
                    [FromBody] BasketCheckoutDto model)
        {
            var result = await _checkoutService.CheckoutOrder(username, model);
            return Accepted(result);
        }

    }
}
