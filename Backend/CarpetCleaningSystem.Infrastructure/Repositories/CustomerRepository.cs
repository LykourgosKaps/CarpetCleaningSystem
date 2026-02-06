using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Domain.Entities;
using CarpetCleaningSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDBContext _context;

        public CustomerRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Customer customer, CancellationToken ct)
        {
            await _context.Customers.AddAsync(customer, ct);
        }

        public async Task<bool> ExistsByPhoneAsync(string phoneNumber, int excludeCustomerId, CancellationToken ct)
        {
            return await _context.Customers
                .AnyAsync(x => (x.PhoneNumber == phoneNumber && x.CustomerId != excludeCustomerId), ct);
        }

        public async Task<Customer?> GetByPhoneAsync(string phoneNumber, CancellationToken ct)
        {
            var phone = phoneNumber.Trim();

            return await _context.Customers
                .FirstOrDefaultAsync(x =>
                    x.PhoneNumber != null &&
                    x.PhoneNumber.Replace("\u00A0", "").Trim() == phone,
                    ct);
        }


        public async Task<Customer?> GetByIdAsync(int customerId, CancellationToken ct)
        {
            return await _context.Customers
                .FindAsync(new object[] { customerId }, ct);
        }
    }
}
