using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Customers.LookupCustomerByPhone
{
    public class LookupCustomerByPhoneQuery
    {
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class LookupCustomerByPhoneResponse
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        
        // Optional: Latest order info if useful for dashboard
        public int? LatestOrderId { get; set; }
        public string? LatestOrderStatus { get; set; }
    }
}
