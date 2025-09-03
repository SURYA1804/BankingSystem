using interfaces;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace Controllers
{
    [ApiController]
    [Route("api/v1/UserRole")]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRolesService _userRolesService;

        public UserRolesController(IUserRolesService userRolesService)
        {
            _userRolesService = userRolesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _userRolesService.GetAllRolesAsync();
            if (roles == null || !roles.Any())
                return NotFound("No roles found.");
            
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] MasterRoles role)
        {
            if (role == null || string.IsNullOrWhiteSpace(role.RoleName))
                return BadRequest("Role data is invalid.");

            var result = await _userRolesService.AddRolesAsync(role);
            if (!result)
                return StatusCode(500, "Failed to add role.");

              return CreatedAtAction(nameof(GetAllRoles), new { roleName = role.RoleName },role);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest("Invalid role name.");

            var result = await _userRolesService.RemoveRolesAsync(roleName);
            if (!result)
                return NotFound($"Role '{roleName}' not found or could not be deleted.");

            return Ok($"Role '{roleName}' deleted successfully.");
        }
    }
}
