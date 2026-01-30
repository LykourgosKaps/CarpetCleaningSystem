using CarpetCleaningSystem.Domain.Enums;
using System;

namespace CarpetCleaningSystem.Domain.Entities
{
    public class OrderItem
    {
        public int OrderItemId { get; private set; } // PK (DB identity)

        public decimal Width { get; private set; }
        public decimal Length { get; private set; }

        public int ItemNo { get; private set; }

        public ItemType ItemType { get; private set; }  

        public CleaningType CleaningType { get; private set; }

        private OrderItem() { } // For EF

        private OrderItem(int itemNo, decimal width, decimal length, ItemType itemType, CleaningType cleaningType)
        {
            if (itemNo <= 0) throw new ArgumentException("Item number must be greater than 0.", nameof(itemNo));
            if (width <= 0) throw new ArgumentException("Width must be greater than 0.", nameof(width));
            if (length <= 0) throw new ArgumentException("Length must be greater than 0.", nameof(length));

            if (!Enum.IsDefined(typeof(ItemType), itemType))
                throw new ArgumentException("Invalid material.", nameof(itemType));

            if (!Enum.IsDefined(typeof(CleaningType), cleaningType))
                throw new ArgumentException("Invalid cleaning type.", nameof(cleaningType));

            ItemNo = itemNo;
            Width = width;
            Length = length;
            ItemType = itemType;
            CleaningType = cleaningType;
        }

        internal static OrderItem Create(int itemNo, decimal width, decimal length, ItemType itemType, CleaningType cleaningType)
            => new OrderItem(itemNo, width, length, itemType, cleaningType);

        public void ChangeDimensions(decimal newWidth, decimal newLength)
        {
            if (newWidth <= 0) throw new ArgumentException("Width must be greater than 0.", nameof(newWidth));
            if (newLength <= 0) throw new ArgumentException("Length must be greater than 0.", nameof(newLength));

            if (Width == newWidth && Length == newLength) return;

            Width = newWidth;
            Length = newLength;
        }

        public void ChangeMaterial(ItemType newMaterial)
        {
            if (!Enum.IsDefined(typeof(ItemType), newMaterial))
                throw new ArgumentException("Invalid material.", nameof(newMaterial));

            if (ItemType == newMaterial) return;

            ItemType = newMaterial;
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

