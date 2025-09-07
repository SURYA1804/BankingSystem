using DTO;
using interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Manager")]
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

        [Authorize(Roles = "Manager")]
        [HttpGet("GetAllStaff")]
        public async Task<IActionResult> GetAllStaff()
        {
            var staffs = await managerService.GetAllStaffAsync();
            if (staffs == null)
            {
                return BadRequest();
            }
            return Ok(staffs);
        }

        [Authorize(Roles = "Manager")]

        [HttpGet("GetAllUserActivity")]
        public async Task<IActionResult> GetAllUserActivity()
        {
            var userActivity = await managerService.GetUserActivityAsync();
            if (userActivity == null)
            {
                return NotFound();
            }
            return Ok(userActivity);
        }

    }
}
