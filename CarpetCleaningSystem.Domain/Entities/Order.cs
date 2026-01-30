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

        // Persisted locked pricing
        public decimal TotalPrice { get; private set; }
        public DateTime? PriceLockedAt { get; private set; }
        private bool IsPriceLocked => PriceLockedAt != null;

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

            TotalPrice = 0m;
            PriceLockedAt = null;
        }

        public static Order Create(int customerId, DateTime pickUpDate)
            => new Order(customerId, pickUpDate);

        // Helper to get next ItemNo
        private int GetNextItemNo()
        {
            return _items.Any()
                ? _items.Max(i => i.ItemNo) + 1
                : 1;
        }

        private void EnsureCanEditItems()
        {
            if (Status != OrderStatus.DRAFT && Status != OrderStatus.SUBMITTED)
                throw new InvalidOperationException(CannotEditItemsMessage);

   
        }

        private OrderItem GetItemOrThrowByItemNo(int itemNo)
        {
            if (itemNo <= 0)
                throw new ArgumentException("ItemNo must be greater than 0.", nameof(itemNo));

            var item = _items.FirstOrDefault(i => i.ItemNo == itemNo);
            if (item is null)
                throw new ArgumentException("Order item not found.", nameof(itemNo));

            return item;
        }

        public void AddItem(decimal width, decimal length, ItemType itemType, CleaningType cleaningType)
        {
            EnsureCanEditItems();

            var itemNo = GetNextItemNo();

            var item = OrderItem.Create(
                itemNo,
                width,
                length,
                itemType,
                cleaningType);

            _items.Add(item);
        }

        public void RemoveItem(int itemNo)
        {
            EnsureCanEditItems();
            var item = GetItemOrThrowByItemNo(itemNo);

            _items.Remove(item);
        }

        // These are the 4 methods - endpoints will call
        public void ChangeItemDimensions(int itemNo, decimal newWidth, decimal newLength)
        {
            EnsureCanEditItems();
            var item = GetItemOrThrowByItemNo(itemNo);
            item.ChangeDimensions(newWidth, newLength);
        }

        public void ChangeItemMaterial(int itemNo, ItemType newMaterial)
        {
            EnsureCanEditItems();
            var item = GetItemOrThrowByItemNo(itemNo);
            item.ChangeMaterial(newMaterial);
        }

        public void ChangeItemCleaningType(int itemNo, CleaningType newCleaningType)
        {
            EnsureCanEditItems();
            var item = GetItemOrThrowByItemNo(itemNo);
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

        // One place to lock prices (by ItemNo)
        public void LockPrices(decimal totalPrice, IReadOnlyDictionary<int, decimal> itemPricesByItemNo)
        {
            if (Status != OrderStatus.SUBMITTED)
                throw new InvalidOperationException("Prices can only be locked when the order is Submitted.");

            if (_items.Count == 0)
                throw new InvalidOperationException("Cannot lock prices for an order without items.");

            if (IsPriceLocked)
                throw new InvalidOperationException("Order prices are already locked.");

            if (itemPricesByItemNo is null)
                throw new ArgumentNullException(nameof(itemPricesByItemNo));

            // Ensure dictionary matches exactly the current items by ItemNo
            var itemNos = _items.Select(i => i.ItemNo).ToHashSet();
            if (!itemNos.SetEquals(itemPricesByItemNo.Keys))
                throw new InvalidOperationException("Item prices must be provided for all order items and only those items.");

            if (totalPrice < 0)
                throw new ArgumentException("Total price cannot be negative.", nameof(totalPrice));

            foreach (var item in _items)
            {
                var price = itemPricesByItemNo[item.ItemNo];
                item.SetLockedPrice(price); // internal method on OrderItem
            }

            TotalPrice = totalPrice;
            PriceLockedAt = DateTime.UtcNow;
        }

        
        public void StartProcessing(decimal totalPrice, IReadOnlyDictionary<int, decimal> itemPricesByItemNo)
        {
            if (Status != OrderStatus.SUBMITTED)
                throw new InvalidOperationException("Order can only move to InProgress from Submitted status.");

            // lock first 
            LockPrices(totalPrice, itemPricesByItemNo);

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



