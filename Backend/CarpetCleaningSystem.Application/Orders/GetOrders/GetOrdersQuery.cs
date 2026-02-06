using CarpetCleaningSystem.Domain.Enums;
using System;
using System.Collections.Generic;

namespace CarpetCleaningSystem.Application.Orders.GetOrders
{
    public class GetOrdersQuery
    {
        public OrderStatus? Status { get; set; }
    }

    public class GetOrdersResponse
    {
        public List<OrderSummaryDTO> Orders { get; set; } = new();
    }

    public class OrderSummaryDTO
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
