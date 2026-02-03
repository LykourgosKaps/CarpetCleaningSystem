using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Exceptions
{
    public class OrderItemsLockedException : Exception
    {
        public OrderItemsLockedException()
            : base("Order items cannot be modified after processing has started.")
        {
        }
    }

}
