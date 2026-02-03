using GamingPlatformAPI.DTO;
using GamingPlatformAPI.iService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GamingPlatformAPI.Controllers
{
    [ApiController]                    
    [Route("api/[controller]")]
    [Authorize]
    public class UserController :ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Get paginated list of users with optional search and filters
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUsers(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] bool? isBlocked = null)
        {
            try
            {
                var agentId = GetAgentIdFromToken();
                if (agentId == 0)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                if (page < 1) page = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var result = await _userService.GetUsersAsync(agentId, page, pageSize, search, isBlocked);

                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users");
                return StatusCode(500, new { message = "An error occurred while fetching users" });
            }
        }

        /// <summary>
        /// Get user details by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var agentId = GetAgentIdFromToken();
                if (agentId == 0)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var user = await _userService.GetUserByIdAsync(id, agentId);

                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(new
                {
                    success = true,
                    data = user
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user details");
                return StatusCode(500, new { message = "An error occurred while fetching user details" });
            }
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var agentId = GetAgentIdFromToken();
            if (agentId == 0)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var user = await _userService.CreateUserAsync(dto, agentId);

            return Ok(new
            {
                success = true,
                data = user
            });
        }


        /// <summary>
        /// Update user details
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                var agentId = GetAgentIdFromToken();
                if (agentId == 0)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var user = await _userService.UpdateUserAsync(id, updateUserDto, agentId);

                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(new
                {
                    success = true,
                    message = "User updated successfully",
                    data = user
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                return StatusCode(500, new { message = "An error occurred while updating user" });
            }
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetDataAsync();
            return Ok(users);
        }
        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _userService.CountAsync();
            return Ok(count);
        }


        /// <summary>
        /// Toggle user block status (block/unblock)
        /// </summary>
        [HttpPatch("{id}/toggle-block")]
        public async Task<IActionResult> ToggleBlockStatus(int id)
        {
            try
            {
                var agentId = GetAgentIdFromToken();  // your auth logic
                if (agentId == 0)
                    return Unauthorized();

                var success = await _userService.ToggleBlockStatusAsync(id, agentId);

                if (!success)
                    return NotFound(new { message = "User not found" });

                return Ok(new { success = true, message = "Status toggled successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling user status");
                return StatusCode(500, new { message = "An error occurred" });
            }
        }


        /// <summary>
        /// Delete a user
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var agentId = GetAgentIdFromToken();
                if (agentId == 0)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var success = await _userService.DeleteUserAsync(id, agentId);

                if (!success)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(new
                {
                    success = true,
                    message = "User deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return StatusCode(500, new { message = "An error occurred while deleting user" });
            }
        }

        private int GetAgentIdFromToken()
        {
            var agentIdClaim = User.FindFirst("AgentId") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            if (agentIdClaim != null && int.TryParse(agentIdClaim.Value, out int agentId))
            {
                return agentId;
            }
            return 0;
        }

    }
}
