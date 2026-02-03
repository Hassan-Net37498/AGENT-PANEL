using GamingPlatformAPI.DTO;

namespace GamingPlatformAPI.iService
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardStatsAsync(int agentId);
        Task<WeeklyEarningsDto> GetWeeklyEarningsAsync(int agentId);
    }
}

