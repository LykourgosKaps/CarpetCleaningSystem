using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Exceptions
{
    public class OrderItemNotFoundException : Exception
    {
        public OrderItemNotFoundException(int orderItemId)
            : base($"Order item with ID {orderItemId} was not found.")
        {
        }
    }
}
