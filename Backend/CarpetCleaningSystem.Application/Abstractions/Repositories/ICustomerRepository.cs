using CarpetCleaningSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Abstractions.Repositories
{
    public interface ICustomerRepository
    {
        Task<bool> ExistsByPhoneAsync(string phoneNumber, int excludeCustomerId, CancellationToken ct);
        Task<Customer?> GetByPhoneAsync(string phoneNumber, CancellationToken ct);
        Task AddAsync(Customer customer, CancellationToken ct);

        Task<Customer?> GetByIdAsync(int customerId, CancellationToken ct);
    }
}
