using CarpetCleaningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Infrastructure.Persistence.EntityTypeConfiguration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // Table name
            builder.ToTable("Customers");

            // Primary Key
            builder.HasKey(x => x.CustomerId);

            // Properties - Auto increment
            builder.Property(x => x.CustomerId)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength (50);

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(50);

            // Unique index
            builder.HasIndex(x => x.PhoneNumber)
                .IsUnique();
        }
    }
}
