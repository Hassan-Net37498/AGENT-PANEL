using GamingPlatformAPI.DTO;

namespace GamingPlatformAPI.iService
{
    public interface ICommissionService
    {
        Task<CommissionSummaryDto> GetCommissionSummaryAsync(int agentId);
        Task<PaginatedResult<CommissionDto>> GetCommissionsAsync(int agentId, int page, int pageSize, DateTime? startDate = null, DateTime? endDate = null, bool? isPaid = null);
        Task<DateRangeCommissionsDto> GetCommissionsByDateRangeAsync(int agentId, DateTime startDate, DateTime endDate);
        Task<List<MonthlyEarningsDto>> GetMonthlyEarningsAsync(int agentId, int months = 12);
    }
}

