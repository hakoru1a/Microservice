using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Exceptions
{
    public class NotFound : ApplicationException
    {
        public NotFound(string entity, object key)
            : base(message: $"Entity \"{entity}\" ({key}) was not found.")
        {
        }
    }
}
