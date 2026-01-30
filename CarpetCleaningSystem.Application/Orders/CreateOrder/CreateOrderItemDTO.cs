using CarpetCleaningSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.CreateOrder
{
    public class CreateOrderItemDTO
    {
        [Range(0.01, 100.00)]
        public decimal Width { get; set; }

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
