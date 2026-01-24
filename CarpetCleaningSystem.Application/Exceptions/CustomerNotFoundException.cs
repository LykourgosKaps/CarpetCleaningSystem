using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Exceptions
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException(int customerId)
            : base($"Customer with ID {customerId} was not found.")
        {
        }
    }
}
