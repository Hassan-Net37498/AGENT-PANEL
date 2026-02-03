using GamingPlatformAPI.DTO;
using GamingPlatformAPI.iService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingPlatformAPI.Controllers
{
    [ApiController]                     // مهم جداً لتفعيل ميزات الـ API مثل ModelState validation
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Agent login endpoint
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Invalid request data", errors = ModelState });
                }

                var result = await _authService.LoginAsync(request);

                if (result == null)
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                _logger.LogInformation($"Agent {result.Email} logged in successfully");

                return Ok(new
                {
                    success = true,
                    message = "Login successful",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, new { message = "An error occurred during login" });
            }
        }

        /// <summary>
        /// Agent registration endpoint
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Invalid request data", errors = ModelState });
                }

                // Check if email already exists
                if (await _authService.EmailExistsAsync(request.Email))
                {
                    return BadRequest(new { message = "Email already exists" });
                }

                var result = await _authService.RegisterAsync(request);

                if (result == null)
                {
                    return BadRequest(new { message = "Registration failed" });
                }

                _logger.LogInformation($"New agent registered: {result.Email}");

                return Ok(new
                {
                    success = true,
                    message = "Registration successful",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return StatusCode(500, new { message = "An error occurred during registration" +ex.Message});
            }
        }

        /// <summary>
        /// Forgot password - dummy endpoint
        /// </summary>
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            try
            {
                // This is a dummy implementation
                _logger.LogInformation($"Password reset requested for: {request.Email}");

                return Ok(new
                {
                    success = true,
                    message = "Password reset link has been sent to your email (dummy response)"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in forgot password");
                return StatusCode(500, new { message = "An error occurred" });
            }
        }
    }

   
}
