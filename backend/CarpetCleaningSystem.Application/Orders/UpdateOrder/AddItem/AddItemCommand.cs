using CarpetCleaningSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarpetCleaningSystem.Application.Orders.UpdateOrder.AddItem
{
    public class AddItemCommand
    {
        public int OrderId { get; set; }

        [Required]
        [Range(0.01, 100.00)]
        public decimal Width { get; set; }

        [Required]
        [Range(0.01, 100.00)]
        public decimal Length { get; set; }

        [Required]
        [EnumDataType(typeof(ItemType))]
        public ItemType Material { get; set; }

        [Required]
        [EnumDataType(typeof(CleaningType))]
        public CleaningType CleaningType { get; set; }
    }
}
