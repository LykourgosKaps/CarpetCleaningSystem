using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Domain.Entities;
using CarpetCleaningSystem.Domain.Enums;
using CarpetCleaningSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        // Implementation of repository methods goes here

        private readonly AppDBContext _context;

        public OrderRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order, CancellationToken ct)
        {
            await _context.Orders.AddAsync(order, ct);
        }

        public async Task<Order?> GetByIdAsync(int orderId, CancellationToken ct)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.OrderId == orderId, ct);
        }

        public async Task<IReadOnlyCollection<Order>> ListAsync(OrderStatus? status, CancellationToken ct)
        {
            var query = _context.Orders
                .Include(o => o.Items)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            return await query
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync(ct);
        }
    }
}
