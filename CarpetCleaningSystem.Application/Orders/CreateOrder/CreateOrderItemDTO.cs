using CarpetCleaningSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.CreateOrder
{
    public class CreateOrderItemDTO
    {
        public decimal Width { get; set; }

        public decimal Length { get; set; }

        public CarpetMaterial Material { get; set; }

        public CleaningType CleaningType { get; set; }
    }
}
