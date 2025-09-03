using Microsoft.AspNetCore.Mvc;
using interfaces;
using Model;

namespace Controllers
{
    [ApiController]
    [Route("api/v1/CustomerType")]
    public class CustomerTypeController : ControllerBase
    {
        private readonly ICustomerTypeService _customerTypeService;

        public CustomerTypeController(ICustomerTypeService customerTypeService)
        {
            _customerTypeService = customerTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomerTypes()
        {
            var types = await _customerTypeService.GetAllCustomerType();
            if (types.Count == 0)
            {
                return NotFound("No Customer Type Found!");
            } 
            return Ok(types);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomerType([FromBody] MasterCustomerType customerType)
        {
            if (customerType == null || string.IsNullOrWhiteSpace(customerType.CustomerType))
                return BadRequest("Invalid customer type.");

            var success = await _customerTypeService.AddCustomerType(customerType);
            if (!success)
                return StatusCode(500, "Failed to add customer type.");

            return CreatedAtAction(nameof(GetAllCustomerTypes), new { id = customerType.CustomerTypeId }, customerType);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCustomerType(string CustomerType)
        {
            if (string.IsNullOrWhiteSpace(CustomerType))
                return BadRequest("Invalid customer type name.");

            var success = await _customerTypeService.DeleteCustomerType(CustomerType);
            if (!success)
                return NotFound("Customer type not found or could not be deleted.");

            return NoContent();
        }
    }
}
