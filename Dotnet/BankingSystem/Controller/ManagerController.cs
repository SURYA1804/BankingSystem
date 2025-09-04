using DTO;
using interfaces;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Controllers
{
    [ApiController]
    [Route("api/v1/Manager")]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService managerService;

        public ManagerController(IManagerService managerService)
        {
            this.managerService = managerService;
        }

        [HttpPost("create-staff")]
        public async Task<IActionResult> CreateStaff([FromBody] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid staff data.");

            var result = await managerService.CreateStaffAsync(registerDTO);

            if (!result)
                return StatusCode(500, "Failed to create staff.");

            return Ok("Staff created successfully.");
        }
    }
}
