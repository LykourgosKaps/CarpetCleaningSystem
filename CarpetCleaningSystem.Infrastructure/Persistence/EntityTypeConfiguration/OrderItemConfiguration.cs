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

            // PK = CarpetLabelNumber (shared with Carpet)
            builder.HasKey(x => x.CarpetLabelNumber);

            builder.Property(x => x.CarpetLabelNumber)
                   .ValueGeneratedNever();

            builder.Property(x => x.CleaningType)
                   .IsRequired()
                   .HasConversion<int>();

            builder.Property(x => x.Price)
                   .IsRequired()
                   .HasPrecision(18, 2);

            // ---------- Relation to Order (shadow FK) ----------
            builder.Property<int>("OrderId");

            builder.HasIndex("OrderId");

            builder.HasOne<Order>()
                   .WithMany("Items")
                   .HasForeignKey("OrderId")
                   .OnDelete(DeleteBehavior.Cascade);

            // ---------- Relation to Carpet (1-1, shared PK) ----------
            builder.HasOne<Carpet>()
                   .WithOne()
                   .HasForeignKey<OrderItem>(x => x.CarpetLabelNumber)
                   .HasPrincipalKey<Carpet>(c => c.CarpetLabelNumber)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
