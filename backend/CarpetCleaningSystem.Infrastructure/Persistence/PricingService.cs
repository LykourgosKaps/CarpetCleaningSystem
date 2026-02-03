using CarpetCleaningSystem.Application.Abstractions.Services;
using CarpetCleaningSystem.Application.Pricing;
using CarpetCleaningSystem.Domain.Enums;
using System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarpetCleaningSystem.Infrastructure.Services
{
    public class PricingService : IPricingService
    {
        public decimal CalculateItemPrice(ItemPricingInput input)
        {
            if (input.Surface <= 0)
                throw new ArgumentException("Surface must be greater than zero.", nameof(input));

            var cleaningFlatPrice = GetCleaningFlatPrice(input.CleaningType);
            var materialRatePerSqm = GetMaterialRatePerSqm(input.ItemType);

            var price = 
                cleaningFlatPrice * 
                materialRatePerSqm * 
                input.Surface;

            return decimal.Round(price, 2, MidpointRounding.AwayFromZero);
        }

        public decimal CalculateOrderTotal(IEnumerable<ItemPricingInput> items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            var total = items.Sum(CalculateItemPrice);

            return decimal.Round(total, 2, MidpointRounding.AwayFromZero);
        }

        // -------------------- Pricing tables --------------------

        private static decimal GetCleaningFlatPrice(CleaningType cleaningType) => cleaningType switch
        {
            CleaningType.STANDARD_CLEAN_AND_RETURN => 4.00m,
            CleaningType.STANDARD_CLEAN_AND_STORAGE => 4.50m,
            CleaningType.DRY_CLEAN_AND_RETURN => 6.50m,
            CleaningType.DRY_CLEAN_AND_STORAGE => 7.00m,
            CleaningType.STORAGE_ONLY => 3.50m,
            _ => throw new ArgumentOutOfRangeException(
                nameof(cleaningType),
                cleaningType,
                "Unsupported cleaning type."
            )
        };

        private static decimal GetMaterialRatePerSqm(ItemType itemType) => itemType switch
        {
            ItemType.SYNTHETIC => 1.20m,
            ItemType.HANDMADE_WOOL => 1.50m,
            ItemType.MACHINE_MADE_WOOL => 2.00m,
            _ => throw new ArgumentOutOfRangeException(
                nameof(itemType),
                itemType,
                "Unsupported item material."
            )
        };
    }
}

