using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Constracts.Domain;
using System.Diagnostics.CodeAnalysis;
using Constracts.Common.Events;
using Order.Domain.OrderAggregate.Events;
using Shared.Enums.Order;

namespace Order.Domain.Entities
{
    public class Order : AuditableEventEntity<long>
    {
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string UserName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Column(TypeName = "nvarchar(250)")]
        public string EmailAddress { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [AllowNull]
        public string? ShippingAddress { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [AllowNull]
        public string? InvoiceAddress { get; set; } 

        public EOrderStatus Status { get; set; } = EOrderStatus.New;

        public Guid No { get; set; } = Guid.NewGuid();

        public Order CreatedOrder()
        {
            AddDomainEvent(new OrderCreatedEvent(TotalPrice, UserName, 
                FirstName, LastName, EmailAddress, ShippingAddress, 
                InvoiceAddress, Status, No.ToString()));
            return this;
        }

        public Order DeletedOrder()
        {
            AddDomainEvent(new OrderDeletedEvent<long>(Id));
            return this;
        }
    }
}
