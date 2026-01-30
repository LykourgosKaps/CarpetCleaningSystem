using CarpetCleaningSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.GetOrderById
{
    public class GetOrderItemResponse
    {
        public int ItemNo { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public decimal Surface { get; set; }
        public ItemType ItemType { get; set; }
        public CleaningType CleaningType { get; set; }
        public decimal ItemPrice { get; set; }
    }
}
