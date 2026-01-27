using CarpetCleaningSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.GetOrderById
{
    public class OrderItemResponse
    {
        public int CarpetLabelNumber { get; }

        public CleaningType CleaningType { get; }

        public decimal Price { get; }

        public OrderItemResponse(
            int carpetLabelNumber,
            CleaningType cleaningType,
            decimal price)
        {
            CarpetLabelNumber = carpetLabelNumber;
            CleaningType = cleaningType;
            Price = price;
        }
    }
}
