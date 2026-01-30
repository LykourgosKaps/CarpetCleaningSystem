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
        public DateTime PickUpDate { get; private set; }
        public DateTime? DeliveryDate { get; private set; }

        private const string CannotEditItemsMessage =
            "Cannot modify order items unless the order is in Draft or Submitted status.";

        // Optional helper
        public decimal GetTotalSurface() => _items.Sum(i => i.Surface);

        private Order() { } // For EF

        private Order(int customerId, DateTime pickUpDate)
        {
            if (customerId <= 0)
                throw new ArgumentException("CustomerId must be greater than zero.", nameof(customerId));

            if (pickUpDate == DateTime.MinValue)
                throw new ArgumentException("Pick-up date must be a valid date.", nameof(pickUpDate));

            CustomerId = customerId;
            CreatedAt = DateTime.UtcNow;
            PickUpDate = pickUpDate;
            Status = OrderStatus.DRAFT;
        }

        public static Order Create(int customerId, DateTime pickUpDate)
            => new Order(customerId, pickUpDate);

        private void EnsureCanEditItems()
        {
            if (Status != OrderStatus.DRAFT && Status != OrderStatus.SUBMITTED)
                throw new InvalidOperationException(CannotEditItemsMessage);
        }

        private OrderItem GetItemOrThrow(int orderItemId)
        {
            if (orderItemId <= 0)
                throw new ArgumentException("OrderItemId must be greater than 0.", nameof(orderItemId));

            var item = _items.FirstOrDefault(i => i.OrderItemId == orderItemId);
            if (item is null)
                throw new ArgumentException("Order item not found.", nameof(orderItemId));

            return item;
        }

        public void AddItem(OrderItem item)
        {
            EnsureCanEditItems();
            ArgumentNullException.ThrowIfNull(item);

            _items.Add(item);
        }

        public void RemoveItem(int orderItemId)
        {
            EnsureCanEditItems();
            var item = GetItemOrThrow(orderItemId);

            _items.Remove(item);
        }

        


        // These are the 4 methods - endpoints will call
        public void ChangeItemDimensions(int orderItemId, decimal newWidth, decimal newLength)
        {
            EnsureCanEditItems();
            var item = GetItemOrThrow(orderItemId);

            item.ChangeDimensions(newWidth, newLength);
        }

        public void ChangeItemMaterial(int orderItemId, ItemType newMaterial)
        {
            EnsureCanEditItems();
            var item = GetItemOrThrow(orderItemId);

            item.ChangeMaterial(newMaterial);
        }

        public void ChangeItemCleaningType(int orderItemId, CleaningType newCleaningType)
        {
            EnsureCanEditItems();
            var item = GetItemOrThrow(orderItemId);

            item.ChangeCleaningType(newCleaningType);
        }

        public void ChangePickUpDate(DateTime newPickUpDate)
        {
            EnsureCanEditItems();

            if (newPickUpDate == DateTime.MinValue)
                throw new ArgumentException("Pick-up date must be a valid date.", nameof(newPickUpDate));

            if (PickUpDate == newPickUpDate) return;

            PickUpDate = newPickUpDate;
        }


        public void Submit()
        {
            if (Status != OrderStatus.DRAFT)
                throw new InvalidOperationException("Only draft orders can be submitted.");

            if (_items.Count == 0)
                throw new InvalidOperationException("Cannot submit an order without items.");

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
            if (Status == OrderStatus.DRAFT || Status == OrderStatus.SUBMITTED)
                Status = OrderStatus.CANCELLED;
            else
                throw new InvalidOperationException("Only Draft or Submitted orders can be cancelled.");

        }


    }
}


