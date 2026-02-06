using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Customers.LookupCustomerByPhone
{
    public class LookupCustomerByPhoneHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;

        public LookupCustomerByPhoneHandler(ICustomerRepository customerRepository, IOrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }

        public async Task<LookupCustomerByPhoneResponse> Handle(LookupCustomerByPhoneQuery query, CancellationToken ct)
        {
            var phone = new string((query.PhoneNumber ?? string.Empty)
                .Trim()
                .Where(char.IsDigit)
                .ToArray());


            var customer = await _customerRepository.GetByPhoneAsync(phone, ct);

            if (customer is null)
                throw new CustomerNotFoundException($"No customer found with phone number: {query.PhoneNumber}");


            var response = new LookupCustomerByPhoneResponse
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address
            };

            // Try to get latest order for this customer
            try
            {
                var allOrders = await _orderRepository.ListAsync(null, ct);
                var customerOrders = allOrders
                    .Where(o => o.CustomerId == customer.CustomerId)
                    .OrderByDescending(o => o.CreatedAt)
                    .FirstOrDefault();

                if (customerOrders != null)
                {
                    response.LatestOrderId = customerOrders.OrderId;
                    response.LatestOrderStatus = customerOrders.Status.ToString();
                }
            }
            catch
            {
                // If we can't get orders, just return customer info
            }

            return response;
        }
    }
}
