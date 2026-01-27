using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Application.Exceptions;
using CarpetCleaningSystem.Domain.Entities;
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

        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateCustomerResponse> Handle(CreateCustomerCommand request, CancellationToken ct)
        {
            var phone = request.PhoneNumber.Trim();

            var exists = await _customerRepository.ExistsByPhoneAsync(phone, excludeCustomerId: 0,ct);

            if (exists) throw new PhoneNumberAlreadyInUseException(phone);

            var customer = Customer.Create(
                request.FirstName.Trim(),
                request.LastName.Trim(),
                phone,
                request.Address.Trim());

            // Add the new customer to the repository
            await _customerRepository.AddAsync(customer, ct);

            // Transaction boundary of the use case
            await _unitOfWork.SaveChangesAsync(ct);

            return new CreateCustomerResponse(customer.CustomerId);
        }
    }
}
