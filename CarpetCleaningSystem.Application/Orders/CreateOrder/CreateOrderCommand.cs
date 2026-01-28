using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.CreateOrder
{
    public class CreateOrderCommand
    {
        [Required]
        [MinLength(1)]
        public List<CreateOrderItemDTO> Items { get; set; } = new();

        [Required]
        public DateTime PickUpDate { get; set; }
    }
}
