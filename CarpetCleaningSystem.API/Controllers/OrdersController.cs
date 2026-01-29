using CarpetCleaningSystem.Application.Orders.CreateOrder;
using CarpetCleaningSystem.Application.Orders.GetOrderById;
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

        public OrdersController(CreateOrderHandler handler, GetOrderByIdHandler getOrderByIdHandler)
        {
            _handler = handler;
            _getOrderHandler = getOrderByIdHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken ct)
        {
            var response = await _handler.Handle(command, ct);
            return Created(string.Empty, response);

        }

        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetOrderById(int orderId, CancellationToken ct)
        {
            var response = await _getOrderHandler.Handle(
                new GetOrderByIdQuery { OrderId = orderId }, ct);

            return Ok(response);
        }

    }
}
