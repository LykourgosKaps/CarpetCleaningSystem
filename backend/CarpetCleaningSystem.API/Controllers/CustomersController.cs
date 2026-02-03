using CarpetCleaningSystem.Application.Customers.CreateCustomer;
using CarpetCleaningSystem.Application.Customers.GetCustomerById;
using CarpetCleaningSystem.Application.Customers.UpdateCustomer;
using CarpetCleaningSystem.Application.Customers.GetCustomerByPhone;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace CarpetCleaningSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Employee")]

    public class CustomersController : ControllerBase
    {
        private readonly CreateCustomerHandler _handler;

        private readonly GetCustomerByIdHandler _getHandler;

        private readonly UpdateCustomerHandler _updateHandler;

        private readonly GetCustomerByPhoneHandler _lookupHandler;

        public CustomersController(CreateCustomerHandler handler, GetCustomerByIdHandler getHandler, UpdateCustomerHandler updateCustomerHandler, GetCustomerByPhoneHandler lookupHandler)
        {
            _handler = handler;
            _getHandler = getHandler;
            _updateHandler = updateCustomerHandler;
            _lookupHandler = lookupHandler;
        }

        [HttpGet("lookup")]
        public async Task<IActionResult> Lookup([FromQuery] string phone, CancellationToken ct)
        {
            var response = await _lookupHandler.Handle(new GetCustomerByPhoneQuery { PhoneNumber = phone }, ct);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand command, CancellationToken ct)
        {
            var response = await _handler.Handle(command, ct);
            return CreatedAtAction(
                nameof(GetCustomerById),
                new { customerId = response.CustomerId },
                response);


        }

        [HttpGet("{customerId:int}")]
        public async Task<IActionResult> GetCustomerById(int customerId, CancellationToken ct)
        {
            var query = new GetCustomerByIdQuery { CustomerId = customerId };

            var response = await _getHandler.Handle(query, ct);
            return Ok(response);
        }

        [HttpPut("{customerId:int}")]
        public async Task<IActionResult> UpdateCustomer(int customerId, [FromBody] UpdateCustomerCommand command, CancellationToken ct)
        {
            await _updateHandler.Handle(command, customerId, ct);
            return NoContent();
        }
    }
}
