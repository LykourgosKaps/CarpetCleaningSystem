using CarpetCleaningSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Domain.Entities
{
    public class Order
    {
        public int OrderId { get; private set; }

        public int CustomerId { get; private set; }

        /* Navigation property from OrderItems to Items
         * in order ONLY I CAN MAKE CHANGES WITH METHODS
         * and this is the reason why private
         */

        private readonly List<OrderItem> _items = new(); // _items εσωτερικη κατασταση

        // Expose OrderItems as a read-only collection where no one can have access
        public IReadOnlyCollection<OrderItem> Items => _items; // Εξωτερικη κατασταση public read-only view

        public OrderStatus Status { get; private set; }

        public DateTime CreatedAt { get; private set; } 

        public DateTime? PickUpDate { get; private set; }

        public DateTime? DeliveryDate { get; private set; }

        // αν θελω μπορει να αλλαξει απο config ευκολα μετα
        private static readonly decimal MinimumCharge = 30m;



        private Order() { } // For ORM tools

        private Order(int customerId)
        {
            if (customerId > 0) this.CustomerId = customerId;
            else throw new ArgumentException("A customerId must be greater than zero.", nameof(customerId));

            // Auto set CreatedAt and Status properties beacuse they are not given from outside (system generated)
            CreatedAt = DateTime.UtcNow;

            Status = OrderStatus.DRAFT;

        }

        public static Order CreateOrder(int customerId)
        { 

            return new Order (customerId);
        }

        private OrderItem GetItemOrThrow(int carpetLabelNumber)
        {
            if (carpetLabelNumber <= 0)
                throw new ArgumentException("Label number must be greater than 0.", nameof(carpetLabelNumber));

            var item = _items.FirstOrDefault(i => i.CarpetLabelNumber == carpetLabelNumber);
            return item ?? throw new InvalidOperationException("No item with the given label number exists in the order.");
        }

        public void AddItem(OrderItem item) 
        {
            if (Status != OrderStatus.DRAFT)
                throw new InvalidOperationException("Cannot add items to an order that is not Draft status.");

            ArgumentNullException.ThrowIfNull(item);

            
            if (_items.Any(i => i.CarpetLabelNumber == item.CarpetLabelNumber))
                throw new InvalidOperationException("Item with the same label number already exists in the order.");

            _items.Add(item);
        }

        public void RemoveItem(int labelNumber)
        {
            if (Status != OrderStatus.DRAFT)
                throw new InvalidOperationException("Cannot remove items from an order that is not Draft status.");

            if (_items.Count == 0)
                throw new InvalidOperationException("Cannot remove item from an empty order.");

            var itemToRemove = GetItemOrThrow(labelNumber);

            _items.Remove(itemToRemove);
        }


        public void ChangeItemCleaningType(int carpetLabelNumber, CleaningType newType, decimal newPrice)
        {
            if (Status != OrderStatus.DRAFT)
                throw new InvalidOperationException("Cannot change items in an order that is not Draft status.");

            var item = GetItemOrThrow(carpetLabelNumber);

            item.ChangeCleaningType(newType, newPrice);
        }


        public void SetPickUpDate (DateTime pickUpDate)
        {
            // State protection
            if ( Status != OrderStatus.DRAFT)
                throw new InvalidOperationException("Pick-up date can only be set while the order is in Draft status.");

            // Input validation
            // pickUpDate cannot be default value - must be a valid date
            if (pickUpDate == DateTime.MinValue)
                throw new ArgumentException("Pick-up date must be a valid date.", nameof(pickUpDate));

            // Business rule
            // pickUpDate cannot be earlier than CreatedAt
            if (pickUpDate < CreatedAt)
                throw new ArgumentException("Pick-up date cannot be earlier than the order creation date.", nameof(pickUpDate));


            PickUpDate = pickUpDate;
        }

        public decimal GetTotalPrice()
        {
            return Math.Max(_items.Sum(i => i.Price), MinimumCharge);
        }


        public void Submit()
        {
            if (Status != OrderStatus.DRAFT) throw new InvalidOperationException("An order cannot pass to Submitted status if it is not in Draft status.");

            if (_items.Count == 0) throw new InvalidOperationException("Cannot submit an order without items.");

            if (PickUpDate == null) throw new InvalidOperationException("Cannot submit an order without a pick-up date.");

            Status = OrderStatus.SUBMITTED;
        }

        public void StartProcessing()
        {
            if ( Status != OrderStatus.SUBMITTED ) throw new InvalidOperationException("Order can only move to InProgress from Submitted status.");

            Status = OrderStatus.IN_PROGRESS;
        }

        public void Complete()
        {
            if (Status != OrderStatus.IN_PROGRESS) throw new InvalidOperationException("Order can only move to Completed from InProgress status.");

            Status = OrderStatus.COMPLETED;

            DeliveryDate = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status != OrderStatus.DRAFT) throw new InvalidOperationException("Order can only move to Cancelled from Draft status.");

            Status=OrderStatus.CANCELLED;
        }
    }
}
