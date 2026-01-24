using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Customers.GetCustomerById
{
    public class GetCustomerByIdHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByIdHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<GetCustomerByIdResponse> Handle(GetCustomerByIdQuery request, CancellationToken ct)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, ct);
            if (customer is null)
                throw new CustomerNotFoundException(request.CustomerId);

            return new GetCustomerByIdResponse(
                customer.CustomerId,
                customer.FirstName,
                customer.LastName,
                customer.PhoneNumber,
                customer.Address);
        }
    }
}
