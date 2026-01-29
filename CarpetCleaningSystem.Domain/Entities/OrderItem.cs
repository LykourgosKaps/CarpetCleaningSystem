using CarpetCleaningSystem.Domain.Enums;
using System;

namespace CarpetCleaningSystem.Domain.Entities
{
    public class OrderItem
    {
        // Primary Key (DB identity)
        public int OrderItemId { get; private set; }

        // FK προς Carpet (null μέχρι να γίνει παραλαβή / submit)
        public int? CarpetId { get; private set; }
        public Carpet? Carpet { get; private set; }

        // Τι ζήτησε ο πελάτης
        public CleaningType CleaningType { get; private set; }

        private OrderItem() { } // For EF

        private OrderItem(CleaningType cleaningType)
        {
            if (!Enum.IsDefined(typeof(CleaningType), cleaningType))
                throw new ArgumentException("Invalid cleaning type.", nameof(cleaningType));

            CleaningType = cleaningType;
            CarpetId = null;
        }

        public static OrderItem Create(CleaningType cleaningType)
            => new OrderItem(cleaningType);

        // Καλείται στο Submit / Receive flow
        public void AssignCarpet(Carpet carpet)
        {
            ArgumentNullException.ThrowIfNull(carpet);

            if (CarpetId.HasValue)
                throw new InvalidOperationException("Order item already assigned to a carpet.");

            Carpet = carpet;
            CarpetId = carpet.CarpetId;
        }

        public void ChangeCleaningType(CleaningType newCleaningType)
        {
            if (!Enum.IsDefined(typeof(CleaningType), newCleaningType))
                throw new ArgumentException("Invalid cleaning type.", nameof(newCleaningType));

            if (CleaningType == newCleaningType) return;

            CleaningType = newCleaningType;
        }
    }
}
