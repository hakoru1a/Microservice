using AutoMapper;
using MediatR;
using Order.Application.Common.Models;
using Order.Domain.Interfaces;
namespace Order.Application.Features.V1.Orders.Queries.GetOrders
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, ApiResult<List<OrderDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;

        public GetOrdersQueryHandler(IMapper mapper, IOrderRepository repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ApiResult<List<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orderEntities = await _repository.GetOrdersByUsername(request.UserName);
            var orderList = _mapper.Map<List<OrderDto>>(orderEntities);
            return new ApiSuccessResult<List<OrderDto>>(orderList);
        }
    }
}
