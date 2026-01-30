using CarpetCleaningSystem.Application.Abstractions.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.CancelOrder
{
    public class CancelOrderHandler
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelOrderHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CancelOrderCommand command, CancellationToken ct)
        {
            if (command.OrderId <= 0)
                throw new ArgumentException("OrderId must be greater than 0.", nameof(command.OrderId));

            var order = await _orderRepository.GetByIdAsync(command.OrderId, ct)
                ?? throw new InvalidOperationException("Order not found.");

            order.Cancel();

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}

