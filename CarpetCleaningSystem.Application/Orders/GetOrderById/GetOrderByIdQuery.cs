using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.GetOrderById
{
    public class GetOrderByIdQuery
    {
        [Required]
        public int OrderId { get; set; }
    }
}
