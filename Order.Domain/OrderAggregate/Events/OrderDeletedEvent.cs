using Constracts.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.OrderAggregate.Events
{
    public class OrderDeletedEvent<T> : BaseEvent
    {
        T Id { get; set; }

        public OrderDeletedEvent(T id)
        {
            Id = id;
        }
    }
}
