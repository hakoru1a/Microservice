using Constracts.Common.Interface;
using Constracts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constracts.Common.Events
{
    public class EventEntity<T> : EntityBase<T>, IEventEntity<T>
    {
        private List<BaseEvent> _events = new();
        public IReadOnlyCollection<BaseEvent> DomainEvents => _events.AsReadOnly();

        public void AddDomainEvent(BaseEvent domainEvent)
        {
            _events.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _events.Clear();
        }

        public void RemoveDomainEvent(BaseEvent domainEvent)
        {
            _events.Remove(domainEvent);
        }
    }
}
