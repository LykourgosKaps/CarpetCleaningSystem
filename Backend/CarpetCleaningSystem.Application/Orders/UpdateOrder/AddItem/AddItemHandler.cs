using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Domain.Entities;
using CarpetCleaningSystem.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.UpdateOrder.AddItem
{
    public class AddItemHandler
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddItemHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AddItemCommand command, CancellationToken ct)
        {
            var order = await _orderRepository.GetByIdAsync(command.OrderId, ct);
            if (order == null) return;

            var item = new OrderItem
            {
                ItemNo = order.Items.Count + 1,
                Width = command.Width,
                Length = command.Length,
                ItemType = (ItemType)command.Material,
                CleaningType = (CleaningType)command.CleaningType,
                OrderId = order.OrderId
            };

            order.Items.Add(item);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
