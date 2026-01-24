using CarpetCleaningSystem.Application.Customers.CreateCustomer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarpetCleaningSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CreateCustomerHandler _handler;

        public CustomersController(CreateCustomerHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand command, CancellationToken ct)
        {
            var response = await _handler.Handle(command, ct);
            return Created(string.Empty, response);

        }
    }
}
