using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Application.Orders.CreateOrder;
using CarpetCleaningSystem.Domain.Entities;

public class CreateOrderHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        // Create order (Draft)
        var order = Order.Create(request.CustomerId, request.PickUpDate);

        foreach (var item in request.Items)
        {
            order.AddItem(
                item.Width,
                item.Length,
                item.Material,
                item.CleaningType);
        }


        // Persist
        await _orderRepository.AddAsync(order, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        // Return only the id (CQRS clean)
        return new CreateOrderResponse
        {
            OrderId = order.OrderId
        };
    }
}

