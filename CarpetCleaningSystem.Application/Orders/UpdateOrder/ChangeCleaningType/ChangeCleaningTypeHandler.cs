using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Application.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

public class ChangeCleaningTypeHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeCleaningTypeHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ChangeCleaningTypeCommand request, CancellationToken ct)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, ct)
            ?? throw new OrderNotFoundException(request.OrderId);

        try
        {
            order.ChangeItemCleaningType(request.ItemNo, request.CleaningType);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        catch (ArgumentException ex) when (ex.ParamName == "itemNo")
        {
            throw new OrderItemNotFoundException(request.ItemNo);
        }
    }
}
