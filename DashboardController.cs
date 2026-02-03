using GamingPlatformAPI.iService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GamingPlatformAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<DashboardController> _logger;
        public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;

        }
        /// <summary>
        /// Get dashboard statistics for the logged-in agent
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

                var stats = await _dashboardService.GetDashboardStatsAsync(agentId);

                return Ok(new
                {
                    success = true,
                    data = stats
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard stats");
                return StatusCode(500, new { message = "An error occurred while fetching statistics" });
            }
        }

        /// <summary>
        /// Get 7-day earnings chart data for the logged-in agent
        /// </summary>
        [HttpGet("earnings")]
        public async Task<IActionResult> GetWeeklyEarnings()
        {
            try
            {
                var agentId = GetAgentIdFromToken();
                if (agentId == 0)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var earnings = await _dashboardService.GetWeeklyEarningsAsync(agentId);

                return Ok(new
                {
                    success = true,
                    data = earnings
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting weekly earnings");
                return StatusCode(500, new { message = "An error occurred while fetching earnings data" });
            }
        }

        /// <summary>
        /// Extract agent ID from JWT token
        /// </summary>
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
