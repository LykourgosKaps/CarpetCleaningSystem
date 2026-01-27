using CarpetCleaningSystem.Application.Orders.CreateOrder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarpetCleaningSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly CreateOrderHandler _handler;

        public OrdersController(CreateOrderHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken ct)
        {
            var response = await _handler.Handle(command, ct);
            return Created(string.Empty, response);

        }
    }
}
