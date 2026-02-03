using CarpetCleaningSystem.Domain.Enums;

namespace CarpetCleaningSystem.Application.Pricing
{
    public record ItemPricingInput(
        decimal Surface,
        ItemType ItemType,
        CleaningType CleaningType
    );
}

