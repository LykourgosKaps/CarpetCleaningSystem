using CarpetCleaningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarpetCleaningSystem.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            // PK (identity)
            builder.HasKey(o => o.OrderId);

            builder.Property(o => o.OrderId)
                   .ValueGeneratedOnAdd();

            // Enum -> int
            builder.Property(o => o.Status)
                   .IsRequired()
                   .HasConversion<int>();

            builder.Property(o => o.PickUpDate)
                   .IsRequired();

            builder.Property(o => o.DeliveryDate)
                   .IsRequired(false);

            // ---- Items mapping (backing field) ----
            // Το Order έχει private List<OrderItem> _items
            // και public IReadOnlyCollection<OrderItem> Items
            builder.Metadata
                   .FindNavigation(nameof(Order.Items))!
                   .SetPropertyAccessMode(PropertyAccessMode.Field);

            // (Η σχέση Order -> OrderItems δηλώνεται από την πλευρά του OrderItemConfiguration)
        }
    }
}

