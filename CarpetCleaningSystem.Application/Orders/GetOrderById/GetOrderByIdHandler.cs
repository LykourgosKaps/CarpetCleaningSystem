using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Application.Orders.GetOrderById;

public class GetOrderByIdHandler
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<GetOrderByIdResponse> Handle(GetOrderByIdQuery query, CancellationToken ct)
    {
        var order = await _orderRepository.GetByIdAsync(query.OrderId, ct)
            ?? throw new InvalidOperationException("Order not found.");

        return new GetOrderByIdResponse
        {
            OrderId = order.OrderId,
            CustomerId = order.CustomerId,
            PickUpDate = order.PickUpDate,
            Status = order.Status.ToString(),
            Items = order.Items.Select(i => new GetOrderItemResponse
            {
                Width = i.Width,
                Length = i.Length,
                Surface = i.Surface,
                ItemType = i.Material,
                CleaningType = i.CleaningType
            }).ToList()
        };
    }
}

