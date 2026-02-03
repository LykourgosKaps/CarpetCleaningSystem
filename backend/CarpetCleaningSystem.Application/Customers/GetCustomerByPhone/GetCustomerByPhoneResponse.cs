using System;

namespace CarpetCleaningSystem.Application.Customers.GetCustomerByPhone
{
    public class GetCustomerByPhoneResponse
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int? LatestOrderId { get; set; }
        public string? LatestOrderStatus { get; set; }
    }
}
