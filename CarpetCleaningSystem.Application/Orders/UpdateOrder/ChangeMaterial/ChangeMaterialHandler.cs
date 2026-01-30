using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.UpdateOrder.ChangeMaterial
{
    public class ChangeMaterialHandler
    {
        private readonly IOrderRepository _orderRepository;

        private readonly IUnitOfWork _unitOfWork;
        public ChangeMaterialHandler(
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ChangeMaterialCommand request, CancellationToken ct)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, ct)
                ?? throw new OrderNotFoundException(request.OrderId);

            try
            {
                order.ChangeItemMaterial(request.OrderItemId, request.newMaterial);
                await _unitOfWork.SaveChangesAsync(ct);
            }
            catch (ArgumentException ex) when (ex.ParamName == "orderItemId")
            {
                throw new OrderItemNotFoundException(request.OrderItemId);
            }
        }
    }
}
