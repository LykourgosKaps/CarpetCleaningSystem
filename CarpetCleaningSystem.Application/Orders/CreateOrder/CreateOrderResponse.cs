using CarpetCleaningSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.CreateOrder
{
    public class CreateOrderResponse
    {
        public int CustomerId { get; set; }
        public int OrderId { get; set; }

        public List<CreateOrderItemResponseDTO> Items { get; set; } = new();

        public DateTime PickUpDate { get; set; }

        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; }

    }
}
