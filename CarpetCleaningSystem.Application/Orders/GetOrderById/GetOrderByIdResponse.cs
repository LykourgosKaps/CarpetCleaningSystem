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
        public int OrderId { get; }

        public int CustomerId { get; }
        
        public OrderStatus Status { get; }

        public DateTime? PickUpDate { get; }

        public DateTime? DeliveryDate { get; }

        public decimal TotalPrice { get; }

        public IReadOnlyCollection<OrderItemResponse> Items { get; }

        public GetOrderByIdResponse(int orderId, int customerId, OrderStatus status, DateTime? pickUpDate, DateTime? deliveryDate, decimal totalPrice, IEnumerable<OrderItemResponse> items)
        {
            OrderId = orderId;
            CustomerId = customerId;
            Status = status;
            PickUpDate = pickUpDate;
            DeliveryDate = deliveryDate;
            TotalPrice = totalPrice;
            Items = [.. items]; // [.. ] syntax to create a read-only collection - similar ToList()
        }

    }
}
