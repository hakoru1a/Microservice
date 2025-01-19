using AutoMapper;
using EventBus.Messages.IntergrationEvent.Event;
using MassTransit;
using MediatR;
using Order.Application.Features.V1.Orders.Commands.CreateOrder;
using ILogger = Serilog.ILogger;

namespace Order.API.Application.IntergateEvents.EventsHandler
{
    public class BasketCheckoutHandler : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public BasketCheckoutHandler(IMediator mediator, IMapper mapper, ILogger logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = _mapper.Map<CreateOrderCommand>(context.Message);
            var result = await _mediator.Send(command);
            _logger.Information("BasketCheckoutEvent consumed successfully. Created Order Id : {newOrderId}", result);
        }
    }
}