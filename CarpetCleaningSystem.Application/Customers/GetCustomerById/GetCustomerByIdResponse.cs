using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Customers.GetCustomerById
{
    public class GetCustomerByIdResponse
    {
        public int CustomerId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string PhoneNumber { get; }
        public string Address { get; }

        public GetCustomerByIdResponse(
            int customerId,
            string firstName,
            string lastName,
            string phoneNumber,
            string address)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Address = address;
        }
    }

}
