using CarpetCleaningSystem.Domain.Enums;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Domain.Entities
{
    public class Customer
    {
        public int CustomerId { get; private set; }
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public string PhoneNumber { get; private set; } = null!;
        public string Address { get; private set; } = null!;

        public CustomerStatus Status { get; private set; }

        public Customer() { }

        private Customer (int customerId, string firstName, string lastName, string phoneNumber, string address) 
        {
            this.CustomerId = customerId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PhoneNumber = phoneNumber;
            this.Address = address;
        }

        public static Customer Create(int customerId, string firstName, string lastName, string phoneNumber, string address)
        {
            if (customerId <= 0) 
                throw new ArgumentException("Customer ID must be a positive integer.", nameof(customerId));

            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty.", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be empty.", nameof(phoneNumber));

            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Address cannot be empty.", nameof(address));

            var customer = new Customer(customerId, firstName, lastName, phoneNumber, address)
            {
                Status = CustomerStatus.Active // Default status
            };
            return customer;
        }

        public void UpdateName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty.", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty.", nameof(lastName));
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public void UpdateContactInfo(string phoneNumber, string address)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be empty.", nameof(phoneNumber));
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Address cannot be empty.", nameof(address));
            this.PhoneNumber = phoneNumber;
            this.Address = address;
        }

        public void Activate()
        {
            Status = CustomerStatus.Active;
        }

        public void Deactivate()
        {
            Status = CustomerStatus.Deactivated;
        }
    }
}
