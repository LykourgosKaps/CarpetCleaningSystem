using CarpetCleaningSystem.Application.Customers.CreateCustomer;
using CarpetCleaningSystem.Application.Customers.GetCustomerById;
using CarpetCleaningSystem.Application.Customers.UpdateCustomer;
using CarpetCleaningSystem.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace CarpetCleaningSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CreateCustomerHandler _handler;

        private readonly GetCustomerByIdHandler _getHandler;

        private readonly UpdateCustomerHandler _updateHandler;

        public CustomersController(CreateCustomerHandler handler, GetCustomerByIdHandler getHandler, UpdateCustomerHandler updateCustomerHandler)
        {
            _handler = handler;
            _getHandler = getHandler;
            _updateHandler = updateCustomerHandler;
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
