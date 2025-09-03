using DTO;
using interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("api/v1/Users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (registerDTO == null)
                return BadRequest("Invalid user data.");

            var isRegistered = await userService.RegisterCustomerAsync(registerDTO);

            if (!isRegistered)
                return StatusCode(500, "Registration failed. Please try again.");

            return CreatedAtAction(nameof(Register), new { email = registerDTO.Email },
                new { message = "Registration successful. Please check your email for OTP." });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromQuery] string email, [FromQuery] int otp)
        {
            var isVerified = await userService.VerifyOtpAsync(email, otp);

            if (!isVerified)
                return BadRequest("Invalid or expired OTP.");

            return Ok(new { message = "Email verified successfully!" });
        }
    }
}
