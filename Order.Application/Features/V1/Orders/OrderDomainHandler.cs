using MediatR;
using Order.Domain.OrderAggregate.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Features.V1.Orders
{
    internal class OrderDomainHandler : INotificationHandler<OrderCreatedEvent>, INotificationHandler<OrderDeletedEvent<long>>
    {
        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            var a = 1;
            return Task.CompletedTask;
        }

        public Task Handle(OrderDeletedEvent<long> notification, CancellationToken cancellationToken)
        {
            var b = 2;
            return Task.CompletedTask;
        }
    }
}
