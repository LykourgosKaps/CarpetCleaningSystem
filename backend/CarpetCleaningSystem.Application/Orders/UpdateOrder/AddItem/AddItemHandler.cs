using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Application.Orders.UpdateOrder.AddItem;
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

        public async Task Handle(AddItemCommand request, CancellationToken ct)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, ct);
            if (order == null)
            {
                throw new System.Exception("Order not found.");
            }

            order.AddItem(
                request.Width,
                request.Length,
                request.Material,
                request.CleaningType);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
