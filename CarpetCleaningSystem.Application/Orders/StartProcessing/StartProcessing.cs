using CarpetCleaningSystem.Application.Orders;
using CarpetCleaningSystem.Application.Abstractions.Repositories;
using CarpetCleaningSystem.Application.Abstractions.Services;
using CarpetCleaningSystem.Application.Pricing;
using CarpetCleaningSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.StartProcessing
{
    public class StartProcessingHandler
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPricingService _pricingService;

        public StartProcessingHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IPricingService pricingService)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _pricingService = pricingService;
        }

        public async Task Handle(StartProcessingCommand command, CancellationToken ct)
        {
            if (command is null) throw new ArgumentNullException(nameof(command));
            if (command.OrderId <= 0) throw new ArgumentException("OrderId must be greater than 0.", nameof(command.OrderId));

            // 1) Load order + items
            var order = await _orderRepository.GetByIdAsync(command.OrderId, ct);
            if (order is null) throw new ArgumentException("Order not found.", nameof(command.OrderId));

            // 2) Build pricing inputs
            var inputsByItemNo = order.Items.ToDictionary(
                i => i.ItemNo,
                i => new ItemPricingInput(
                    i.Surface,
                    i.ItemType,
                    i.CleaningType
                )
            );

            // 3) Calculate item prices (by ItemNo)
            var itemPricesByItemNo = new Dictionary<int, decimal>(capacity: inputsByItemNo.Count);

            foreach (var kvp in inputsByItemNo)
            {
                var itemNo = kvp.Key;
                var input = kvp.Value;

                var price = _pricingService.CalculateItemPrice(input);
                itemPricesByItemNo[itemNo] = price;
            }

            // 4) Total
            var totalPrice = itemPricesByItemNo.Values.Sum();

            // 5) Domain transition (enforces locking inside StartProcessing)
            order.StartProcessing(totalPrice, itemPricesByItemNo);

            // 6) Persist
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
