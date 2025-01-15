using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Exceptions
{
    public class InvalidType : ApplicationException
    {
        public InvalidType(string entity, string type)
            : base(message: $"Entity \"{entity}\" not supported type: {type}")
        {
        }
    }
}
