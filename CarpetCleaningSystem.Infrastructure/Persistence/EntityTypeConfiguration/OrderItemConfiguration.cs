using CarpetCleaningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Infrastructure.Persistence.EntityTypeConfiguration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            // PK (global unique)
            builder.HasKey(x => x.CarpetLabelNumber);

            builder.Property(x => x.CarpetLabelNumber)
                   .IsRequired();

            builder.Property(x => x.CleaningType)
                   .IsRequired()
                   .HasConversion<int>();

            builder.Property(x => x.Price)
                   .IsRequired()
                   .HasPrecision(18, 2);

            // Shadow FK προς Order
            builder.Property<int>("OrderId");

            builder.HasIndex("OrderId");

            builder.HasOne<Order>()
                   .WithMany("Items")
                   .HasForeignKey("OrderId")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
