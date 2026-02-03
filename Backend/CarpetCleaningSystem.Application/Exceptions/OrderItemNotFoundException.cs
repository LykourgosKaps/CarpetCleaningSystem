using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Exceptions
{
    public class OrderItemNotFoundException : Exception
    {
        public OrderItemNotFoundException(int itemNo)
            : base($"Order item with itemNo {itemNo} was not found.")
        {
        }
    }
}
