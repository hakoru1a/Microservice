using Shared.Enums.Order;

namespace Shared.DTOs.Order
{
    public class OrderDto
    {
        public decimal TotalPrice { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string? ShippingAddress { get; set; }

        public string? InvoiceAddress { get; set; }

        public string status { get; set; }

        public string No { get; set; } = Guid.NewGuid().ToString();
    }
}
