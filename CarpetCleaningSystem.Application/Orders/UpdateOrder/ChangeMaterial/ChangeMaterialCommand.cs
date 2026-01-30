using CarpetCleaningSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.UpdateOrder.ChangeMaterial
{
    public class ChangeMaterialCommand
    {

        [Required]
        [Range(1, int.MaxValue)]
        public int OrderId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int OrderItemId { get; set; }

        [Required]
        public ItemType newMaterial { get; set; }
    }
}
