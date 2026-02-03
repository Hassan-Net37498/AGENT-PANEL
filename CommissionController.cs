using GamingPlatformAPI.iService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace GamingPlatformAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommissionController : ControllerBase
    {
        private readonly ICommissionService _commissionService;
        private readonly ILogger<CommissionController> _logger;

        public CommissionController(ICommissionService commissionService, ILogger<CommissionController> logger)
        {
            _commissionService = commissionService;
            _logger = logger;
        }

        /// <summary>
        /// Get commission summary statistics
        /// </summary>
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            try
            {
                var agentId = GetAgentIdFromToken();
                if (agentId == 0)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var summary = await _commissionService.GetCommissionSummaryAsync(agentId);

                return Ok(new
                {
                    success = true,
                    data = summary
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting commission summary");
                return StatusCode(500, new { message = "An error occurred while fetching commission summary" });
            }
        }

        /// <summary>
        /// Get paginated commission history with filters
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCommissions(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] bool? isPaid = null)
        {
            try
            {
                var agentId = GetAgentIdFromToken();
                if (agentId == 0)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                if (page < 1) page = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 20;

                var result = await _commissionService.GetCommissionsAsync(agentId, page, pageSize, startDate, endDate, isPaid);

                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting commissions");
                return StatusCode(500, new { message = "An error occurred while fetching commissions" });
            }
        }

        /// <summary>
        /// Get commissions by date range
        /// </summary>
        [HttpGet("range")]
        public async Task<IActionResult> GetByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var agentId = GetAgentIdFromToken();
                if (agentId == 0)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var result = await _commissionService.GetCommissionsByDateRangeAsync(agentId, startDate, endDate);

                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting commissions by date range");
                return StatusCode(500, new { message = "An error occurred while fetching commissions" });
            }
        }

        /// <summary>
        /// Get monthly earnings for the past N months
        /// </summary>
        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyEarnings([FromQuery] int months = 12)
        {
            try
            {
                var agentId = GetAgentIdFromToken();
                if (agentId == 0)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                if (months < 1 || months > 24) months = 12;

                var result = await _commissionService.GetMonthlyEarningsAsync(agentId, months);

                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting monthly earnings");
                return StatusCode(500, new { message = "An error occurred while fetching monthly earnings" });
            }
        }

        /// <summary>
        /// Export commissions to CSV
        /// </summary>
        [HttpGet("export")]
        public async Task<IActionResult> ExportToCSV(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var agentId = GetAgentIdFromToken();
                if (agentId == 0)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                // Get all commissions for the date range
                var defaultStartDate = startDate ?? DateTime.UtcNow.AddMonths(-3);
                var defaultEndDate = endDate ?? DateTime.UtcNow;

                var result = await _commissionService.GetCommissionsByDateRangeAsync(
                    agentId,
                    defaultStartDate,
                    defaultEndDate);

                // Generate CSV
                var csv = new StringBuilder();
                csv.AppendLine("Date,User Name,User Email,Base Amount,Commission Rate,Commission Amount,Status");

                foreach (var commission in result.Commissions)
                {
                    csv.AppendLine($"{commission.EarnedDate:yyyy-MM-dd HH:mm}," +
                                  $"\"{commission.UserName}\"," +
                                  $"\"{commission.UserEmail}\"," +
                                  $"{commission.BaseAmount:F2}," +
                                  $"{commission.CommissionRate:F2}%," +
                                  $"{commission.Amount:F2}," +
                                  $"{(commission.IsPaid ? "Paid" : "Pending")}");
                }

                var bytes = Encoding.UTF8.GetBytes(csv.ToString());
                var fileName = $"commissions_{defaultStartDate:yyyy-MM-dd}_to_{defaultEndDate:yyyy-MM-dd}.csv";

                return File(bytes, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting commissions");
                return StatusCode(500, new { message = "An error occurred while exporting commissions" });
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
