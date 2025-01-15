using MediatR;
using Order.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Features.V1.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand<long>, ApiResult<bool>>
    {
        private IOrderRepository _repository;
        public DeleteOrderCommandHandler(IOrderRepository orderRepository) {
            _repository = orderRepository;
        }
        public async Task<ApiResult<bool>> Handle(DeleteOrderCommand<long> request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetByIdAsync(request.Id);
            if (order == null)
            {
                return new ApiResult<bool>(false, "Order not found");
            }
            await _repository.DeleteAsync(order);
            return new ApiResult<bool>(true, "Order successfully deleted");
        }
    }   
}
