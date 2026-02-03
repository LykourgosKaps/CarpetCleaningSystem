using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.StartProcessing
{
    public class StartProcessingCommand
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int OrderId { get; set; }
    }
}
