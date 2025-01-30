using AutoMapper;
using MediatR;
using Order.Application.Common.Models;
using Order.Application.Features.V1.Orders.Queries.GetOrders;
using Order.Domain.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Features.V1.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, ApiSuccessResult<OrderDto>>
    {
        private readonly ILogger _logger;
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;


        public GetOrderByIdQueryHandler(ILogger logger, IOrderRepository orderRepository, IMapper mapper)
        {
            _logger = logger;
            _repository = orderRepository;
            _mapper = mapper;
        }


        private const string MethodName = "GetOrderByIdQueryHandler";

        public async Task<ApiSuccessResult<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.Information(messageTemplate: $"BEGIN: {MethodName} - Id: {request.Id}");

            var order = await _repository.GetByIdAsync(request.Id);

            var orderDto = _mapper.Map<OrderDto>(order);

            _logger.Information(messageTemplate: $"END: {MethodName} - Id: {request.Id}");

            return new ApiSuccessResult<OrderDto>(orderDto);
        }
    }
}
