using DTO;
using interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service;

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

            if (isVerified == "User Not Found")
            {
                return BadRequest("User Not Found");
            }
            else if(isVerified == "OTP Not Found")
            {
                return BadRequest("OTP Not Found");
            }
            else if(isVerified == "OTP Expired")
            {
                return BadRequest("OTP Expired");
            }
            else if(isVerified == "failed")
            {
                return StatusCode(500, "Internal Server Error");
            }

            return Ok(new { message = "Email verified successfully!" });
        }

        [HttpGet("Get-OTP")]
        
        public async Task<IActionResult> GetOTP([FromQuery] string email)
        {
            var IsOTPSent = await userService.GetOTPAsync(email);
            if (IsOTPSent)
            {
                return Ok("OTP Sent");
            }
            else
            {
                return BadRequest("Not Sent!"); 
            }
            
        }
    }
}
