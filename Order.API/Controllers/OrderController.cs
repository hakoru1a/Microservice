using Constracts.Messages;
using Constracts.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Common.Models;
using Order.Application.Features.V1.Orders.Commands.CreateOrder;
using Order.Application.Features.V1.Orders.Queries.GetOrders;
using Shared.Services.Email;
using System.ComponentModel.DataAnnotations;

namespace Order.API.Controllers
{
    [ApiController]
    [Route("")]  
    public class OrderController : ControllerBase
    {
       private IMediator mediator;

       private readonly ISMTPEmailServices _emailServices;

        private readonly IMessageProducer _messageProducer;
        public OrderController(IMediator mediator, ISMTPEmailServices emailServices, IMessageProducer messageProducer)
       {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this._emailServices = emailServices;
            this._messageProducer = messageProducer;
        }

        private static class Routes
        {
            public const string GetOrders = nameof(GetOrders);
        }



        [HttpGet("orders/{username}", Name = Routes.GetOrders)]

        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrderCatalog([Required] string username)
        {
            var query = new GetOrdersQuery(username);

            var result = await mediator.Send(query);

            return Ok(result);
        }

        [HttpGet(template: "test-email")]
        public async Task<IActionResult> TestEmail()
        {
            var message = new MailRequest
            {
                Body = "hello",
                Subject = "Test",
                ToAddress = "kietpham.dev@gmail.com"
            };

            await _emailServices.SendEmailAsync(message);

            return Ok();
        }

        [HttpGet("test-create")]
        public async Task<IActionResult> TestCreate()
        {
            var command = new CreateOrderCommand
            {
                UserName = "testuser",
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "john.doe@example.com",
                TotalPrice = 99.99m,
                ShippingAddress = "123 Test Street, Test City, 12345",
                InvoiceAddress = "123 Test Street, Test City, 12345",
                Status = "Pending"
            };
            _messageProducer.SendMessage<CreateOrderCommand>(command);
            var result = await mediator.Send(command);

            return Ok();
        }
    }
}