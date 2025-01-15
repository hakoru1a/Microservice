using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Features.V1.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommand<T> : IRequest<ApiResult<bool>>
    {
        public T Id {set; get; }
    }
}
