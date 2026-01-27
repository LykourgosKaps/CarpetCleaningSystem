using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.CreateOrder
{
    public class CreateOrderHandler
    {
        private readonly IOrderRepository _orderRepository;

        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken ct)
        {
            var order = Order.CreateOrder(request.CustomerId);

            if (request.PickUpDate.HasValue)
            {
                order.SetPickUpDate(request.PickUpDate.Value);
            }

            await _orderRepository.AddAsync(order, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return new CreateOrderResponse(order.OrderId);
        }
    }
}
