using CarpetCleaningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarpetCleaningSystem.Infrastructure.Persistence.Configurations
{
    public class CarpetConfiguration : IEntityTypeConfiguration<Carpet>
    {
        public void Configure(EntityTypeBuilder<Carpet> builder)
        {
            builder.ToTable("Carpets");

            // PK (business identity)
            builder.HasKey(x => x.CarpetLabelNumber);

            builder.Property(x => x.CarpetLabelNumber)
                   .ValueGeneratedNever();

            builder.Property(x => x.Material)
                   .IsRequired()
                   .HasConversion<int>();

            builder.Property(x => x.Length)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.Property(x => x.Width)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.Property(x => x.IsLocked)
                   .IsRequired();

            // Derived value (Length * Width) – δεν αποθηκεύεται
            builder.Ignore(x => x.Surface);
        }
    }
}

