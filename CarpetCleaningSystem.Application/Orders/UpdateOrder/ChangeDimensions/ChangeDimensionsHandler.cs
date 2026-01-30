using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Application.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

public class ChangeDimensionsHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeDimensionsHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ChangeDimensionsCommand request, CancellationToken ct)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, ct)
            ?? throw new OrderNotFoundException(request.OrderId);

        try
        {
            order.ChangeItemDimensions(request.ItemNo, request.Width, request.Length);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        catch (ArgumentException ex) when (ex.ParamName == "itemNo")
        {
            throw new OrderItemNotFoundException(request.ItemNo);
        }
    }
}
