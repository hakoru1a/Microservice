using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Order.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderCatalog = Order.Domain.Entities.Order;
namespace Order.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderCatalog>
    {
        public void Configure(EntityTypeBuilder<OrderCatalog> builder)
        {
            builder.Property(x => x.Status)
                   .HasDefaultValue(EOrderStatus.New)
                   .IsRequired();
        }
    }
}
