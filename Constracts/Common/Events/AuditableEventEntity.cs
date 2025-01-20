using Constracts.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constracts.Common.Events
{
    public abstract class AuditableEventEntity<T> : EventEntity<T>, IAuditable
    {
        public DateTimeOffset CreatedDate { get; set; } 
        public DateTimeOffset? LastModifiedDate { get; set; }
    }
}
