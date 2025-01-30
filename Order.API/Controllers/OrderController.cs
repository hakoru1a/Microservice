using AutoMapper;
using Constracts.Messages;
using Constracts.Services;
using MassTransit.Mediator;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Features.V1.Orders.Commands.CreateOrder;
using Order.Application.Features.V1.Orders.Commands.DeleteOrder;
using Order.Application.Features.V1.Orders.Commands.DeleteOrderByDocumentNo;
using Order.Application.Features.V1.Orders.Queries.GetOrderById;
using Order.Application.Features.V1.Orders.Queries.GetOrders;
using Shared.DTOs.Order;
using Shared.Services.Email;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Order.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]  
    public class OrderController : ControllerBase
    {
        private MediatR.IMediator _mediator;

        private readonly ISMTPEmailServices _emailServices;

        private readonly IMessageProducer _messageProducer;

        private readonly IMapper _mapper;

        private static class Routes
        {
            public const string GetOrders = nameof(GetOrders);
            public const string GetOrder = nameof(GetOrder);
            public const string CreateOrder = nameof(CreateOrder);
            public const string DeleteOrder = nameof(DeleteOrder);
            public const string DeleteOrderByDocumentNo = nameof(DeleteOrderByDocumentNo);
        }

        public OrderController(MediatR.IMediator mediator, ISMTPEmailServices emailServices, IMessageProducer messageProducer, IMapper mapper)
       {
            this._mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this._emailServices = emailServices;
            this._messageProducer = messageProducer;
            this._mapper = mapper;
        }

        [HttpPost(Name = Routes.CreateOrder)]
        [ProducesResponseType(typeof(ApiResult<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResult<long>>> CreateOrder([FromBody] CreateOrderDto model)
        {
            var command = _mapper.Map<CreateOrderCommand>(model);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id:long}", Name = Routes.DeleteOrder)]
        [ProducesResponseType(typeof(ApiResult<bool>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResult<bool>>> DeleteOrder([Required] long id)
        {
            var command = new DeleteOrderCommand<long>(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet(template: "{id:long}", Name = Routes.GetOrder)]
        [ProducesResponseType(typeof(OrderDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiSuccessResult<OrderDto>>> GetOrder([Required] long id)
        {
            var query = new GetOrderByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("orders/{username}", Name = Routes.GetOrders)]

        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrderCatalog([Required] string username)
        {
            var query = new GetOrdersQuery(username);

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpDelete(template: "document-no/{documentNo}", Name = Routes.DeleteOrderByDocumentNo)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.NoContent)]
        public async Task<ApiResult<bool>> DeleteOrderByDocumentNo([Required] string documentNo)
        {
            var command = new DeleteOrderByDocumentNoCommand(documentNo);
            var result = await _mediator.Send(command);
            return result;
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
            var result = await _mediator.Send(command);

            return Ok();
        }
    }
}