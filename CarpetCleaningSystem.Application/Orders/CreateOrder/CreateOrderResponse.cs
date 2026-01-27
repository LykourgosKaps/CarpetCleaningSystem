using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.CreateOrder
{
    public class CreateOrderResponse
    {
        public int OrderId { get; }

        public CreateOrderResponse(int orderId)
        {
            OrderId = orderId;
        }
    }
}
