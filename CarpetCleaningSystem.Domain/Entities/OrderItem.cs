using CarpetCleaningSystem.Domain.Enums;
using System;

namespace CarpetCleaningSystem.Domain.Entities
{
    public class OrderItem
    {
        public int OrderItemId { get; private set; } // PK (DB identity)

        public decimal Width { get; private set; }
        public decimal Length { get; private set; }

        // αυτό που είπες: π.χ. Handmade/Blanket/etc (βάλε enum που έχεις ή θα φτιάξεις)
        public CarpetMaterial Material { get; private set; }  // ή ItemType/CarpetType αν έχεις άλλο enum

        public CleaningType CleaningType { get; private set; }

        private OrderItem() { } // For EF

        private OrderItem(decimal width, decimal length, CarpetMaterial material, CleaningType cleaningType)
        {
            if (width <= 0) throw new ArgumentException("Width must be greater than 0.", nameof(width));
            if (length <= 0) throw new ArgumentException("Length must be greater than 0.", nameof(length));

            if (!Enum.IsDefined(typeof(CarpetMaterial), material))
                throw new ArgumentException("Invalid material.", nameof(material));

            if (!Enum.IsDefined(typeof(CleaningType), cleaningType))
                throw new ArgumentException("Invalid cleaning type.", nameof(cleaningType));

            Width = width;
            Length = length;
            Material = material;
            CleaningType = cleaningType;
        }

        public static OrderItem Create(decimal width, decimal length, CarpetMaterial material, CleaningType cleaningType)
            => new OrderItem(width, length, material, cleaningType);

        public void ChangeDimensions(decimal newWidth, decimal newLength)
        {
            if (newWidth <= 0) throw new ArgumentException("Width must be greater than 0.", nameof(newWidth));
            if (newLength <= 0) throw new ArgumentException("Length must be greater than 0.", nameof(newLength));

            if (Width == newWidth && Length == newLength) return;

            Width = newWidth;
            Length = newLength;
        }

        public void ChangeMaterial(CarpetMaterial newMaterial)
        {
            if (!Enum.IsDefined(typeof(CarpetMaterial), newMaterial))
                throw new ArgumentException("Invalid material.", nameof(newMaterial));

            if (Material == newMaterial) return;

            Material = newMaterial;
        }

        public void ChangeCleaningType(CleaningType newCleaningType)
        {
            if (!Enum.IsDefined(typeof(CleaningType), newCleaningType))
                throw new ArgumentException("Invalid cleaning type.", nameof(newCleaningType));

            if (CleaningType == newCleaningType) return;

            CleaningType = newCleaningType;
        }

        public decimal Surface => Width * Length;
    }
}

