using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constracts.Common.Events;
using Shared.Enums.Order;

namespace Order.Domain.OrderAggregate.Events
{
    public class OrderCreatedEvent : BaseEvent
    {
        public decimal TotalPrice { get; set; }

        public string UserName { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string? ShippingAddress { get; set; }

        public string? InvoiceAddress { get; set; }

        public EOrderStatus Status { get; set; } = EOrderStatus.New;

        public string No { get; set; } 

        public OrderCreatedEvent(decimal totalPrice, string userName, string firstName, string lastName, string emailAddress, string? shippingAddress, string? invoiceAddress, EOrderStatus status, string no)
        {
            TotalPrice = totalPrice;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            ShippingAddress = shippingAddress;
            InvoiceAddress = invoiceAddress;
            Status = status;
            No = no;
        }
    }
}
