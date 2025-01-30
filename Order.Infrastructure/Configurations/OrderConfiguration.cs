using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrderCatalog = Order.Domain.Entities.Order;
using Shared.Enums.Order;
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
