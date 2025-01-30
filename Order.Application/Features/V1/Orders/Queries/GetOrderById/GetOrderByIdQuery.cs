using MediatR;
using Order.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Features.V1.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQuery : IRequest<ApiSuccessResult<OrderDto>>
    {
        public long Id { get; set; }

        public GetOrderByIdQuery(long id)
        {
            Id = id;
        }
    }
}
