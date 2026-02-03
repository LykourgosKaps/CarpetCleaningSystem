using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Customers.UpdateCustomer
{
    public class UpdateCustomerHandler
    {
        private readonly ICustomerRepository _customerRepository;

        private readonly IUnitOfWork _unitOfWork;
        public UpdateCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateCustomerCommand request, int customerId, CancellationToken ct)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId, ct);

            if ( customer == null) throw new CustomerNotFoundException(customerId); 

            if (customer.PhoneNumber != request.PhoneNumber)
            {
                var exists = await _customerRepository.ExistsByPhoneAsync(request.PhoneNumber, customerId,ct);

                if (exists) throw new PhoneNumberAlreadyInUseException(request.PhoneNumber);
            }

            customer.UpdateName(request.FirstName, request.LastName);
            customer.UpdateContactInfo(request.PhoneNumber, request.Address);

            await _unitOfWork.SaveChangesAsync(ct);
        }

    }
}
