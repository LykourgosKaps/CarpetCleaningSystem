using CarpetCleaningSystem.Domain.Entities;
using CarpetCleaningSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Abstractions.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order, CancellationToken cancellationToken);

        Task<Order?> GetByIdAsync(int orderId, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<Order>> ListAsync(OrderStatus? status, CancellationToken cancellationToken);
    }
}
