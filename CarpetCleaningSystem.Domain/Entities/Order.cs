using CarpetCleaningSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarpetCleaningSystem.Domain.Entities
{
    public class Order
    {
        public int OrderId { get; private set; }
        public int CustomerId { get; private set; }

        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items;

        public OrderStatus Status { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? PickUpDate { get; private set; }
        public DateTime? DeliveryDate { get; private set; }

        private Order() { } // For EF

        private Order(int customerId)
        {
            if (customerId <= 0)
                throw new ArgumentException("CustomerId must be greater than zero.", nameof(customerId));

            CustomerId = customerId;
            CreatedAt = DateTime.UtcNow;
            Status = OrderStatus.DRAFT;
        }

        public static Order Create(int customerId)
            => new Order(customerId);

        public void AddItem(OrderItem item)
        {
            if (Status != OrderStatus.DRAFT)
                throw new InvalidOperationException("Cannot add items to an order that is not in Draft status.");

            ArgumentNullException.ThrowIfNull(item);

            _items.Add(item);
        }

        public void RemoveItem(int orderItemId)
        {
            if (Status != OrderStatus.DRAFT)
                throw new InvalidOperationException("Cannot remove items from an order that is not in Draft status.");

            var item = _items.FirstOrDefault(i => i.OrderItemId == orderItemId)
                ?? throw new InvalidOperationException("Order item not found.");

            _items.Remove(item);
        }


        public void SetPickUpDate(DateTime pickUpDate)
        {
            if (Status != OrderStatus.DRAFT)
                throw new InvalidOperationException("Pick-up date can only be set while the order is in Draft status.");

            if (pickUpDate == DateTime.MinValue)
                throw new ArgumentException("Pick-up date must be a valid date.", nameof(pickUpDate));

            if (pickUpDate < CreatedAt)
                throw new ArgumentException("Pick-up date cannot be earlier than order creation date.", nameof(pickUpDate));

            PickUpDate = pickUpDate;
        }

        public void Submit()
        {
            if (Status != OrderStatus.DRAFT)
                throw new InvalidOperationException("Only draft orders can be submitted.");

            if (_items.Count == 0)
                throw new InvalidOperationException("Cannot submit an order without items.");

            if (PickUpDate == null)
                throw new InvalidOperationException("Cannot submit an order without a pick-up date.");

            Status = OrderStatus.SUBMITTED;
        }

        public void StartProcessing()
        {
            if (Status != OrderStatus.SUBMITTED)
                throw new InvalidOperationException("Order can only move to InProgress from Submitted status.");

            Status = OrderStatus.IN_PROGRESS;
        }

        public void Complete()
        {
            if (Status != OrderStatus.IN_PROGRESS)
                throw new InvalidOperationException("Order can only be completed from InProgress status.");

            Status = OrderStatus.COMPLETED;
            DeliveryDate = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status != OrderStatus.DRAFT)
                throw new InvalidOperationException("Only draft orders can be cancelled.");

            Status = OrderStatus.CANCELLED;
        }
    }
}

