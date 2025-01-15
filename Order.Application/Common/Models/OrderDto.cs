using Order.Application.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderCatalog = Order.Domain.Entities.Order;

namespace Order.Application.Common.Models
{
    public class OrderDto : IMapFrom<OrderCatalog>
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        //Address
        public string ShippingAddress { get; set; }
        public string InvoiceAddress { get; set; }

        public string Status { get; set; }
    }
}
