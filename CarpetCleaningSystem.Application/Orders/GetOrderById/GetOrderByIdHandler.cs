using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Application.Abstractions.Services;
using CarpetCleaningSystem.Application.Orders.GetOrderById;
using CarpetCleaningSystem.Application.Pricing;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class GetOrderByIdHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPricingService _pricingService;

    public GetOrderByIdHandler(
        IOrderRepository orderRepository,
        IPricingService pricingService)
    {
        _orderRepository = orderRepository;
        _pricingService = pricingService;
    }

    public async Task<GetOrderByIdResponse> Handle(GetOrderByIdQuery query, CancellationToken ct)
    {
        var order = await _orderRepository.GetByIdAsync(query.OrderId, ct)
            ?? throw new InvalidOperationException("Order not found.");

        var isPriceLocked = order.PriceLockedAt != null;

        var response = new GetOrderByIdResponse
        {
            OrderId = order.OrderId,
            CustomerId = order.CustomerId,
            PickUpDate = order.PickUpDate,
            Status = order.Status,
            PriceLockedAt = order.PriceLockedAt
        };

        if (isPriceLocked)
        {
            // configs klLOCKED → persisted prices
            response.Items = order.Items.Select(i => new GetOrderItemResponse
            {
                ItemNo = i.ItemNo,
                Width = i.Width,
                Length = i.Length,
                Surface = i.Surface,
                ItemType = i.ItemType,
                CleaningType = i.CleaningType,
                ItemPrice = i.ItemPrice
            }).ToList();

            response.TotalPrice = order.TotalPrice;
        }
        else
        {
            // NOT LOCKED → computed quote
            decimal total = 0m;

            response.Items = order.Items.Select(i =>
            {
                var input = new ItemPricingInput(
                    i.Surface,
                    i.ItemType,
                    i.CleaningType
                );

                var price = _pricingService.CalculateItemPrice(input);
                total += price;

                return new GetOrderItemResponse
                {
                    ItemNo = i.ItemNo,
                    Width = i.Width,
                    Length = i.Length,
                    Surface = i.Surface,
                    ItemType = i.ItemType,
                    CleaningType = i.CleaningType,
                    ItemPrice = price
                };
            }).ToList();

            response.TotalPrice = decimal.Round(total, 2, MidpointRounding.AwayFromZero);
        }

        return response;
    }
}


