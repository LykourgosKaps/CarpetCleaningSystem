using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Application.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Customers.GetCustomerByPhone
{
    public class GetCustomerByPhoneHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;

        public GetCustomerByPhoneHandler(ICustomerRepository customerRepository, IOrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }

        public async Task<GetCustomerByPhoneResponse> Handle(GetCustomerByPhoneQuery query, CancellationToken ct)
        {
            var customer = await _customerRepository.GetByPhoneAsync(query.PhoneNumber, ct);
            if (customer == null)
            {
                return null!; // Controller will return NotFound
            }

            var latestOrder = await _orderRepository.GetLatestByCustomerIdAsync(customer.CustomerId, ct);

            return new GetCustomerByPhoneResponse
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
                LatestOrderId = latestOrder?.OrderId,
                LatestOrderStatus = latestOrder?.Status.ToString()
            };
        }
    }
}
