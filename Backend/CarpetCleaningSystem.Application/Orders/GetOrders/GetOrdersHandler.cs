using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Domain.Entities;
using CarpetCleaningSystem.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.GetOrders
{
    public class GetOrdersHandler
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;

        public GetOrdersHandler(IOrderRepository orderRepository, ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        public async Task<GetOrdersResponse> Handle(GetOrdersQuery query, CancellationToken ct)
        {
            var orders = await _orderRepository.ListAsync(query.Status, ct);
            
            var orderSummaries = new List<OrderSummaryDTO>();

            foreach (var o in orders)
            {
                var customer = await _customerRepository.GetByIdAsync(o.CustomerId, ct);
                
                orderSummaries.Add(new OrderSummaryDTO
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    CustomerName = customer != null ? $"{customer.FirstName} {customer.LastName}" : "Unknown",
                    PhoneNumber = customer?.PhoneNumber ?? "",
                    Address = customer?.Address ?? "",
                    Status = o.Status,
                    CreatedAt = o.CreatedAt,
                    TotalPrice = o.TotalPrice
                });
            }

            return new GetOrdersResponse
            {
                Orders = orderSummaries
            };
        }
    }
}
