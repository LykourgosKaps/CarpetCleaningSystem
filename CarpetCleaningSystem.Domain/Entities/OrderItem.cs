using CarpetCleaningSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Domain.Entities
{
    public class OrderItem
    {
        public int CarpetLabelNumber { get; private set; }
        public CleaningType CleaningType { get; private set; }
        public decimal Price { get; private set; }



        private OrderItem() { }

        private OrderItem(int carpetLabelNumber, CleaningType cleaningType, decimal price)
        {
            if (carpetLabelNumber <= 0)
                throw new ArgumentException("Carpet label number must be greater than 0.", nameof(carpetLabelNumber));

            if (!Enum.IsDefined(typeof(CleaningType), cleaningType))
                throw new ArgumentException("Invalid cleaning type.", nameof(cleaningType));

            if (price <= 0) throw new ArgumentException("Price must be greater than 0.", nameof(price));



            CarpetLabelNumber = carpetLabelNumber;
            CleaningType = cleaningType;
            Price = price;
        }

        public static OrderItem CreateOrderItem(int carpetLabelNumber, CleaningType cleaningType, decimal price)
        {
            return new OrderItem(carpetLabelNumber, cleaningType, price);
        }

        public void ChangeCleaningType(CleaningType newCleaningType, decimal newPrice)
        {
            if (!Enum.IsDefined(typeof(CleaningType), newCleaningType))
                throw new ArgumentException("Invalid cleaning type.", nameof(newCleaningType));

            if (newPrice <= 0) throw new ArgumentException("Price must be greater than 0.", nameof(newPrice));

            if (CleaningType == newCleaningType && Price == newPrice) return;

            CleaningType = newCleaningType;
            Price = newPrice;
        }

    }
}
