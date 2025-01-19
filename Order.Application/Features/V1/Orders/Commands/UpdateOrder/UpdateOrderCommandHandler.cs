using AutoMapper;
using MediatR;
using Order.Application.Common.Exceptions;
using Order.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderCatalog = Order.Domain.Entities.Order;

namespace Order.Application.Features.V1.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResult<long>>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _repository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResult<long>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new NotFoundException(nameof(request));
            }

            var order = _mapper.Map<OrderCatalog>(request);

            await _repository.UpdateAsync(order);

            return new ApiResult<long>(order.Id);
        }
    }
}
