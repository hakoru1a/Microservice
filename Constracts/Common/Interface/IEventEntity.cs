using Constracts.Common.Events;
using Constracts.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constracts.Common.Interface
{
    public interface IEventEntity
    {
        void AddDomainEvent(BaseEvent domainEvent);
        void RemoveDomainEvent(BaseEvent domainEvent);
        void ClearDomainEvents();
        IReadOnlyCollection<BaseEvent> DomainEvents { get; }
    }

    public interface IEventEntity<T> : IEnityBase<T>, IEventEntity
    {

    }
}
