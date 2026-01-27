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
        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }

        public DateTime? PickUpDate { get; set; }
    }
}
