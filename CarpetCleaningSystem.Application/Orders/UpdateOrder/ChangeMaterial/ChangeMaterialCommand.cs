using CarpetCleaningSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.UpdateOrder.ChangeMaterial
{
    public class ChangeMaterialCommand
    {
        [JsonIgnore]
        public int OrderId { get; set; }
        
        [JsonIgnore]
        public int ItemNo { get; set; }

        [Required]
        [ValidEnum(typeof(ItemType))]
        public ItemType newMaterial { get; set; }
    }
}
