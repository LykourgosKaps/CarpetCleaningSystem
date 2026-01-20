using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Customers.CreateCustomer
{
    public class CreateCustomerResponse
    {
        
        public int CustomerId { get; } 

        public CreateCustomerResponse(int customerId)
        {
            CustomerId = customerId;
        }
    }
}
