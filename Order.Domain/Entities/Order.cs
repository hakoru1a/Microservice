using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Constracts.Domain;
using Order.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace Order.Domain.Entities
{
    public class Order : EntityAuditBase<long>
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
    }
}
