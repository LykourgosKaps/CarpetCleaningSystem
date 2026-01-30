using CarpetCleaningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarpetCleaningSystem.Infrastructure.Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(oi => oi.OrderItemId);

            builder.Property(oi => oi.ItemNo)
                   .IsRequired();

            builder.HasIndex(oi => new { oi.OrderId, oi.ItemNo })
                   .IsUnique();


            builder.Property(oi => oi.Width)
                   .IsRequired()
                   .HasPrecision(9, 2);

            builder.Property(oi => oi.Length)
                   .IsRequired()
                   .HasPrecision(9, 2);

            builder.Property(oi => oi.ItemType)
                   .IsRequired();

            builder.Property(oi => oi.CleaningType)
                   .IsRequired();
        }
    }
}
