using DTO;
using interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace Controllers
{
    [ApiController]
    [Route("api/v1/Users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IAuthService authService;

        public UsersController(IUserService userService, IAuthService authService)
        {
            this.userService = userService;
            this.authService = authService;

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

        [Authorize]
        [HttpGet("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromQuery] string email, [FromQuery] int otp)
        {
            var isVerified = await userService.VerifyOtpAsync(email, otp);

            if (isVerified == "User Not Found")
            {
                return BadRequest("User Not Found");
            }
            else if (isVerified == "OTP Not Found")
            {
                return BadRequest("OTP Not Found");
            }
            else if (isVerified == "OTP Expired")
            {
                return BadRequest("OTP Expired");
            }
            else if (isVerified == "failed")
            {
                return StatusCode(500, "Internal Server Error");
            }

            return Ok(new { message = "Email verified successfully!" });
        }

        [Authorize]
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

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var result = await userService.LoginAsync(loginDTO);
            if (result != null)
            {
                var token = await authService.GenerateJwtToken(result);
                return Ok(new
                {
                    token = token,
                    user = result
                });
            }
            else
            {
                return NotFound("User Not Found");
            }

        }

        [Authorize]
        [HttpPatch("UpdateUserProfile")]
        public async Task<IActionResult> PatchUser(int userId, [FromBody] JsonPatchDocument<UsersModel> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Patch document cannot be null");
            }

            var user = await userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }


            if (!TryValidateModel(user))
            {
                return BadRequest(ModelState);
            }

            patchDoc.ApplyTo(user, ModelState);
            
            await userService.UpdateUserAsync(userId, patchDoc);

            return Ok();
        }

        [Authorize]

        [HttpGet("CheckPassword")]
        public async Task<IActionResult> CheckPassword(int userId, string password)
        {
            var result = await userService.CheckPasswordAsync(userId, password);
            if (result)
            {
                return Ok("true");
            }
            else
            {
                return BadRequest("Password Incorrect");
            }
        }

    }
}
