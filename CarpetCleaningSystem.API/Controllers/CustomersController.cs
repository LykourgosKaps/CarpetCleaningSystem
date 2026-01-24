using CarpetCleaningSystem.Application.Customers.CreateCustomer;
using CarpetCleaningSystem.Application.Customers.GetCustomerById;
using CarpetCleaningSystem.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarpetCleaningSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CreateCustomerHandler _handler;

        private readonly GetCustomerByIdHandler _getHandler;

        public CustomersController(CreateCustomerHandler handler, GetCustomerByIdHandler getHandler)
        {
            _handler = handler;
            _getHandler = getHandler;
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

            try
            {
                var response = await _getHandler.Handle(query, ct);
                return Ok(response);
            }
            catch (CustomerNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

    }
}
