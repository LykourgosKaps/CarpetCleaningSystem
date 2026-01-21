using CarpetCleaningSystem.Application.Abstractions.Repositories;
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

        Task<CreateCustomerResponse> Handle(CreateCustomerCommand request, CancellationToken ct)
        {

            throw new NotImplementedException();
        }
    }
}
