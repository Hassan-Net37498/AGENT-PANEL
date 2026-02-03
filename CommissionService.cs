using GamingPlatformAPI.DTO;
using GamingPlatformAPI.iService;
using GamingPlatformAPI.ORM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GamingPlatformAPI.Service
{
    public class CommissionService : ICommissionService
    {
        private readonly GamingDbContext _context;
        public CommissionService(GamingDbContext dbContext)
        {
            _context = dbContext;

        }
        public async Task<CommissionSummaryDto> GetCommissionSummaryAsync(int agentId)
        {
            var commissions = await _context.Commissions
                .Where(c => c.AgentId == agentId)
                .ToListAsync();

            var totalEarned = commissions.Sum(c => c.Amount);
            var totalPaid = commissions.Where(c => c.IsPaid).Sum(c => c.Amount);
            var totalPending = commissions.Where(c => !c.IsPaid).Sum(c => c.Amount);

            var now = DateTime.UtcNow;
            var thisMonthStart = new DateTime(now.Year, now.Month, 1);
            var lastMonthStart = thisMonthStart.AddMonths(-1);

            var thisMonthEarnings = commissions
                .Where(c => c.EarnedDate >= thisMonthStart)
                .Sum(c => c.Amount);

            var lastMonthEarnings = commissions
                .Where(c => c.EarnedDate >= lastMonthStart && c.EarnedDate < thisMonthStart)
                .Sum(c => c.Amount);

            return new CommissionSummaryDto
            {
                TotalEarned = totalEarned,
                TotalPaid = totalPaid,
                TotalPending = totalPending,
                TotalTransactions = commissions.Count,
                AverageCommission = commissions.Count > 0 ? totalEarned / commissions.Count : 0,
                ThisMonthEarnings = thisMonthEarnings,
                LastMonthEarnings = lastMonthEarnings
            };
        }

        public async Task<PaginatedResult<CommissionDto>> GetCommissionsAsync(
            int agentId,
            int page,
            int pageSize,
            DateTime? startDate = null,
            DateTime? endDate = null,
            bool? isPaid = null)
        {
            var query = _context.Commissions
                .Include(c => c.user)
                .Where(c => c.AgentId == agentId);

            // Apply date filters
            if (startDate.HasValue)
            {
                query = query.Where(c => c.EarnedDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                var endOfDay = endDate.Value.Date.AddDays(1);
                query = query.Where(c => c.EarnedDate < endOfDay);
            }

            // Apply payment status filter
            if (isPaid.HasValue)
            {
                query = query.Where(c => c.IsPaid == isPaid.Value);
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var skip = (page - 1) * pageSize;

            var commissions = await query
                .OrderByDescending(c => c.EarnedDate)
                .Skip(skip)
                .Take(pageSize)
                .Select(c => new CommissionDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.user.FullName,
                    UserEmail = c.user.Email,
                    Amount = c.Amount,
                    BaseAmount = c.BaseAmount,
                    CommissionRate = c.CommissionRate,
                    EarnedDate = c.EarnedDate,
                    IsPaid = c.IsPaid
                })
                .ToListAsync();

            return new PaginatedResult<CommissionDto>
            {
                Items = commissions,
                TotalItems = totalItems,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                HasPrevious = page > 1,
                HasNext = page < totalPages
            };
        }

        public async Task<DateRangeCommissionsDto> GetCommissionsByDateRangeAsync(int agentId, DateTime startDate, DateTime endDate)
        {
            var endOfDay = endDate.Date.AddDays(1);

            var commissions = await _context.Commissions
                .Include(c => c.user)
                .Where(c => c.AgentId == agentId && c.EarnedDate >= startDate && c.EarnedDate < endOfDay)
                .OrderByDescending(c => c.EarnedDate)
                .Select(c => new CommissionDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.user.FullName,
                    UserEmail = c.user.Email,
                    Amount = c.Amount,
                    BaseAmount = c.BaseAmount,
                    CommissionRate = c.CommissionRate,
                    EarnedDate = c.EarnedDate,
                    IsPaid = c.IsPaid
                })
                .ToListAsync();

            return new DateRangeCommissionsDto
            {
                Commissions = commissions,
                TotalAmount = commissions.Sum(c => c.Amount),
                TotalCount = commissions.Count,
                StartDate = startDate,
                EndDate = endDate
            };
        }

        public async Task<List<MonthlyEarningsDto>> GetMonthlyEarningsAsync(int agentId, int months = 12)
        {
            var startDate = DateTime.UtcNow.AddMonths(-months).Date;

            var commissions = await _context.Commissions
                .Where(c => c.AgentId == agentId && c.EarnedDate >= startDate)
                .ToListAsync();

            var monthlyData = commissions
                .GroupBy(c => new { c.EarnedDate.Year, c.EarnedDate.Month })
                .Select(g => new MonthlyEarningsDto
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                    TotalEarnings = g.Sum(c => c.Amount),
                    TransactionCount = g.Count()
                })
                .OrderBy(m => m.Month)
                .ToList();

            return monthlyData;
        }

    }
}
