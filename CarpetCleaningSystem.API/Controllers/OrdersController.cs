using CarpetCleaningSystem.Application.Orders.CancelOrder;
using CarpetCleaningSystem.Application.Orders.CompleteOrder;
using CarpetCleaningSystem.Application.Orders.CreateOrder;
using CarpetCleaningSystem.Application.Orders.GetOrderById;
using CarpetCleaningSystem.Application.Orders.StartProcessing;
using CarpetCleaningSystem.Application.Orders.SubmitOrder;
using CarpetCleaningSystem.Application.Orders.UpdateOrder.ChangeMaterial;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarpetCleaningSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly CreateOrderHandler _handler;

        private readonly GetOrderByIdHandler _getOrderHandler;

        private readonly ChangeCleaningTypeHandler _changeCleaningTypeHandler;

        private readonly ChangeMaterialHandler _changeMaterialHandler;

        private readonly ChangeDimensionsHandler _changeDimensionsHandler;

        private readonly ChangePickUpDateHandler _changePickUpDateHandler;

        private readonly StartProcessingHandler _startProcessingHandler;

        private readonly SubmitOrderHandler _submitOrderHandler;

        private readonly CompleteOrderHandler _completeOrderHandler;

        private readonly CancelOrderHandler _cancelOrderHandler;


        public OrdersController(CreateOrderHandler handler,
                                GetOrderByIdHandler getOrderByIdHandler,
                                ChangeCleaningTypeHandler changeCleaningTypeHandler,
                                ChangeMaterialHandler changeMaterialHandler,
                                ChangeDimensionsHandler changeDimensionsHandler,
                                ChangePickUpDateHandler changePickUpDateHandler,
                                SubmitOrderHandler submitOrderHandler,
                                StartProcessingHandler startProcessingHandler,
                                CompleteOrderHandler completeOrderHandler,
                                CancelOrderHandler cancelOrderHandler)
        {
            _handler = handler;
            _getOrderHandler = getOrderByIdHandler;
            _changeCleaningTypeHandler = changeCleaningTypeHandler;
            _changeMaterialHandler = changeMaterialHandler;
            _changeDimensionsHandler = changeDimensionsHandler;
            _changePickUpDateHandler = changePickUpDateHandler;
            _submitOrderHandler = submitOrderHandler;
            _startProcessingHandler = startProcessingHandler;
            _completeOrderHandler = completeOrderHandler;
            _cancelOrderHandler = cancelOrderHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken ct)
        {
            var response = await _handler.Handle(command, ct);
            return Created(string.Empty, response);

        }

        [HttpGet("{orderId:int}/order")]
        public async Task<IActionResult> GetOrderById(int orderId, CancellationToken ct)
        {
            var response = await _getOrderHandler.Handle(
                new GetOrderByIdQuery { OrderId = orderId }, ct);

            return Ok(response);
        }

        [HttpPost("{orderId:int}/submit")]
        public async Task<IActionResult> SubmitOrder(int orderId, CancellationToken ct)
        {
            await _submitOrderHandler.Handle(new SubmitOrderCommand { OrderId = orderId }, ct);
            return NoContent();
        }

        [HttpPut("{orderId:int}/items/{itemNo:int}/material")]
        public async Task<IActionResult> ChangeMaterial(
            int orderId, int itemNo,
            [FromBody] ChangeMaterialCommand command,
            CancellationToken ct)
        {
            command.OrderId = orderId;
            command.ItemNo = itemNo;

            await _changeMaterialHandler.Handle(command, ct);
            return NoContent();
        }

        [HttpPut("{orderId:int}/items/{itemNo:int}/dimensions")]
        public async Task<IActionResult> ChangeDimensions(
            int orderId, int itemNo,
            [FromBody] ChangeDimensionsCommand command,
            CancellationToken ct)
        {
            command.OrderId = orderId;
            command.ItemNo = itemNo;

            await _changeDimensionsHandler.Handle(command, ct);
            return NoContent();
        }

        [HttpPut("{orderId:int}/items/{itemNo:int}/cleaning-type")]
        public async Task<IActionResult> ChangeCleaningType(
            int orderId, int itemNo,
            [FromBody] ChangeCleaningTypeCommand command,
            CancellationToken ct)
        {
            command.OrderId = orderId;
            command.ItemNo = itemNo;

            await _changeCleaningTypeHandler.Handle(command, ct);
            return NoContent();
        }

        [HttpPut("{orderId:int}/pickup-date")]
        public async Task<IActionResult> ChangePickUpDate(
            int orderId,
            [FromBody] ChangePickUpDateCommand command,
            CancellationToken ct)
        {
            command.OrderId = orderId;

            await _changePickUpDateHandler.Handle(command, ct);
            return NoContent();
        }

        [HttpPost("{orderId:int}/start-processing")]
        public async Task<IActionResult> StartProcessing(int orderId, CancellationToken ct)
        {
            await _startProcessingHandler.Handle(new StartProcessingCommand { OrderId = orderId }, ct);
            return NoContent();
        }

        [HttpPost("{orderId:int}/complete")]
        public async Task<IActionResult> CompleteOrder(int orderId, CancellationToken ct)
        {
            await _completeOrderHandler.Handle(new CompleteOrderCommand { OrderId = orderId }, ct);
            return NoContent();
        }

        [HttpPost("{orderId:int}/cancel")]
        public async Task<IActionResult> CancelOrder(int orderId, CancellationToken ct)
        {
            await _cancelOrderHandler.Handle(new CancelOrderCommand { OrderId = orderId }, ct);
            return NoContent();

        }
    }
}
