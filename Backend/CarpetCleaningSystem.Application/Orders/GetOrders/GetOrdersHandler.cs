using CarpetCleaningSystem.Application.Abstractions.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.GetOrders
{
    public class GetOrdersHandler
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrdersHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<GetOrdersResponse> Handle(GetOrdersQuery query, CancellationToken ct)
        {
            var orders = await _orderRepository.ListAsync(query.Status, ct);
            
            return new GetOrdersResponse
            {
                Orders = orders.Select(o => new OrderSummaryDTO
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt,
                    TotalPrice = o.TotalPrice
                }).ToList()
            };
        }
    }
}
