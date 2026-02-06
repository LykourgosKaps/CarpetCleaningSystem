using CarpetCleaningSystem.Application.Customers.CreateCustomer;
using CarpetCleaningSystem.Application.Customers.GetCustomerById;
using CarpetCleaningSystem.Application.Customers.UpdateCustomer;
using CarpetCleaningSystem.Application.Customers.LookupCustomerByPhone;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace CarpetCleaningSystem.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class CustomersController : ControllerBase
    {
        private readonly CreateCustomerHandler _handler;

        private readonly GetCustomerByIdHandler _getHandler;

        private readonly UpdateCustomerHandler _updateHandler;

        private readonly LookupCustomerByPhoneHandler _lookupHandler;

        public CustomersController(CreateCustomerHandler handler, GetCustomerByIdHandler getHandler, UpdateCustomerHandler updateCustomerHandler, LookupCustomerByPhoneHandler lookupHandler)
        {
            _handler = handler;
            _getHandler = getHandler;
            _updateHandler = updateCustomerHandler;
            _lookupHandler = lookupHandler;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand command, CancellationToken ct)
        {
            var response = await _handler.Handle(command, ct);
            return CreatedAtAction(
                nameof(GetCustomerById),
                new { customerId = response.CustomerId },
                response);


        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{customerId:int}")]
        public async Task<IActionResult> GetCustomerById(int customerId, CancellationToken ct)
        {
            var query = new GetCustomerByIdQuery { CustomerId = customerId };

            var response = await _getHandler.Handle(query, ct);
            return Ok(response);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpPut("{customerId:int}")]
        public async Task<IActionResult> UpdateCustomer(int customerId, [FromBody] UpdateCustomerCommand command, CancellationToken ct)
        {
            await _updateHandler.Handle(command, customerId, ct);
            return NoContent();
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("lookup")]
        public async Task<IActionResult> LookupByPhone([FromQuery] string phone, CancellationToken ct)
        {
            var query = new LookupCustomerByPhoneQuery { PhoneNumber = phone };
            var response = await _lookupHandler.Handle(query, ct);
            return Ok(response);
        }
    }
}
