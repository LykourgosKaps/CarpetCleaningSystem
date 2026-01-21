using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Customers.CreateCustomer
{
    public class CreateCustomerHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        async Task<CreateCustomerResponse> Handle(CreateCustomerCommand request, CancellationToken ct)
        {
            var phone = request.PhoneNumber.Trim();

            var exists = await _customerRepository.ExistsByPhoneAsync(phone, ct);

            if (exists) throw new CustomerAlreadyExistsException(phone);

            throw new NotImplementedException();
        }
    }
}
