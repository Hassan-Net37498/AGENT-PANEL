using GamingPlatformAPI.DTO;
using GamingPlatformAPI.iService;
using GamingPlatformAPI.ORM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GamingPlatformAPI.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly GamingDbContext _context;
        public DashboardService(GamingDbContext context)
        {
            _context = context; 
            
        }
        public async Task<DashboardDto> GetDashboardStatsAsync(int agentId)
        {
            var agent = await _context.Agents
                .Include(a => a.Users)
                .FirstOrDefaultAsync(a => a.Id == agentId);

            if (agent == null)
            {
                throw new Exception("Agent not found");
            }

            var stats = new DashboardDto
            {
                TotalUsers = agent.Users.Count,
                ActiveUsers = agent.Users.Count(u => !u.IsBlocked),
                BlockedUsers = agent.Users.Count(u => u.IsBlocked)
            };

            return stats;
        }

        public async Task<WeeklyEarningsDto> GetWeeklyEarningsAsync(int agentId)
        {
            var startDate = DateTime.UtcNow.Date.AddDays(-6);
            var endDate = DateTime.UtcNow.Date.AddDays(1);

            var earnings = await _context.Commissions
                .Where(c => c.AgentId == agentId && c.Date >= startDate && c.Date < endDate)
                .GroupBy(c => c.Date.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Amount = g.Sum(c => c.Amount)
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            // Fill in missing days with zero
            var chartData = new List<EarningsChartDto>();
            for (int i = 6; i >= 0; i--)
            {
                var date = DateTime.UtcNow.Date.AddDays(-i);
                var dayEarning = earnings.FirstOrDefault(e => e.Date == date);

                chartData.Add(new EarningsChartDto
                {
                    Date = date.ToString("yyyy-MM-dd"),
                    Amount = dayEarning?.Amount ?? 0,
                    Label = date.ToString("MMM dd")
                });
            }

            var totalEarnings = chartData.Sum(c => c.Amount);
            var averageDaily = chartData.Count > 0 ? totalEarnings / chartData.Count : 0;

            return new WeeklyEarningsDto
            {
                Data = chartData,
                TotalEarnings = totalEarnings,
                AverageDaily = averageDaily
            };
        }
    }
}
