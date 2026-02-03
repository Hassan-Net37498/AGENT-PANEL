using GamingPlatformAPI.DTO;
using GamingPlatformAPI.iService;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GamingPlatformAPI.Controllers
{
    public class WithdrawalController : ControllerBase
    {
        private readonly IWithdrawalService _withdrawalService;
        private readonly ILogger<WithdrawalController> _logger;

        public WithdrawalController(IWithdrawalService withdrawalService, ILogger<WithdrawalController> logger)
        {
            _withdrawalService = withdrawalService;
            _logger = logger;
        }

        /// <summary>
        /// Get withdrawal statistics
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            try
            {
                var agentId = GetAgentIdFromToken();
                if (agentId == 0)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var stats = await _withdrawalService.GetWithdrawalStatsAsync(agentId);

                return Ok(new
                {
                    success = true,
                    data = stats
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting withdrawal stats");
                return StatusCode(500, new { message = "An error occurred while fetching withdrawal statistics" });
            }
        }

        /// <summary>
        /// Get withdrawal history with pagination and filters
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetWithdrawals(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? status = null)
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

                var result = await _withdrawalService.GetWithdrawalsAsync(agentId, page, pageSize, status);

                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting withdrawals");
                return StatusCode(500, new { message = "An error occurred while fetching withdrawals" });
            }
        }

        /// <summary>
        /// Get withdrawal details by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWithdrawal(int id)
        {
            try
            {
                var agentId = GetAgentIdFromToken();
                if (agentId == 0)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var withdrawal = await _withdrawalService.GetWithdrawalByIdAsync(id, agentId);

                if (withdrawal == null)
                {
                    return NotFound(new { message = "Withdrawal not found" });
                }

                return Ok(new
                {
                    success = true,
                    data = withdrawal
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting withdrawal details");
                return StatusCode(500, new { message = "An error occurred while fetching withdrawal details" });
            }
        }

        /// <summary>
        /// Create a new withdrawal request
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateWithdrawal([FromBody] CreateWithdrawalDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Invalid data", errors = ModelState });
                }

                var agentId = GetAgentIdFromToken();
                if (agentId == 0)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var withdrawal = await _withdrawalService.CreateWithdrawalAsync(createDto, agentId);

                if (withdrawal == null)
                {
                    return BadRequest(new { message = "Failed to create withdrawal request" });
                }

                return Ok(new
                {
                    success = true,
                    message = "Withdrawal request created successfully",
                    data = withdrawal
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating withdrawal");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cancel a pending withdrawal request
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelWithdrawal(int id)
        {
            try
            {
                var agentId = GetAgentIdFromToken();
                if (agentId == 0)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var success = await _withdrawalService.CancelWithdrawalAsync(id, agentId);

                if (!success)
                {
                    return BadRequest(new { message = "Cannot cancel this withdrawal" });
                }

                return Ok(new
                {
                    success = true,
                    message = "Withdrawal cancelled successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling withdrawal");
                return StatusCode(500, new { message = "An error occurred while cancelling withdrawal" });
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
