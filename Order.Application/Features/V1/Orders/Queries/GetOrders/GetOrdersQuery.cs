using MediatR;
using Order.Application.Common.Models;

namespace Order.Application.Features.V1.Orders.Queries.GetOrders
{
    public class GetOrdersQuery : IRequest<ApiResult<List<OrderDto>>>
    {
        public string UserName { get; private set; }

        public GetOrdersQuery(string userName)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
    }
}
