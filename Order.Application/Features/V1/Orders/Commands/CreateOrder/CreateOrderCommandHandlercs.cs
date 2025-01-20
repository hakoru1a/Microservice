using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Common.Models;
using Order.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;
using OrderCatalog = Order.Domain.Entities.Order;

namespace Order.Application.Features.V1.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;
        private readonly ILogger _logger;

        public CreateOrderCommandHandler(
            IOrderRepository repository,
            IMapper mapper,
            ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        async Task<ApiResult<long>> IRequestHandler<CreateOrderCommand, ApiResult<long>>.Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information("Creating new order for user: {UserName}", request.UserName);

                var orderCataLog = _mapper.Map<OrderCatalog>(request);

                _repository.Create(orderCataLog);
                orderCataLog.CreatedOrder();
               await _repository.SaveChangesAsync();

                _logger.Information("Successfully created order with ID: {OrderId}", orderCataLog.Id);

                return new ApiResult<long>(true, orderCataLog.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while creating order for user: {UserName}", request.UserName);
                throw;
            }
        }
    }
}