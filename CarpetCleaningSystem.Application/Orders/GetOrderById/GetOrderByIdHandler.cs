using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.GetOrderById
{
    public class GetOrderByIdHandler
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderByIdHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<GetOrderByIdResponse> Handle(GetOrderByIdQuery request, CancellationToken ct)
        {
            var order = await _orderRepository.GetByIdWithItemsAsync(request.OrderId, ct);
            if (order is null)
            {
                throw new OrderNotFoundException(request.OrderId);
            }

            var itemResponses = order.Items
                .Select(item => new OrderItemResponse(
                    item.CarpetLabelNumber,
                    item.CleaningType,
                    item.Price
                ))
                .ToList();

            var totalPrice = itemResponses.Sum(x => x.Price);


            return new GetOrderByIdResponse(
                order.OrderId,
                order.CustomerId,
                order.Status,
                order.PickUpDate,
                order.DeliveryDate,
                totalPrice,
                itemResponses);
        }
    }
}
