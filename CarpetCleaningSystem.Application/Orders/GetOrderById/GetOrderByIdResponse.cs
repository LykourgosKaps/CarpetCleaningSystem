using CarpetCleaningSystem.Domain.Entities;
using CarpetCleaningSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.GetOrderById
{
    public class GetOrderByIdResponse
    {
        public int CustomerId { get; set; }
        public int OrderId { get; set; }
        public DateTime PickUpDate { get; set; }
        public OrderStatus Status { get; set; } 
        public List<GetOrderItemResponse> Items { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public DateTime? PriceLockedAt { get; set; }
    }
}
