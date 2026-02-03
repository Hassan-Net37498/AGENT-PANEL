using GamingPlatformAPI.DTO;
using GamingPlatformAPI.iService;
using GamingPlatformAPI.models;
using GamingPlatformAPI.ORM;
using Microsoft.EntityFrameworkCore;

namespace GamingPlatformAPI.Service
{
    public class WithdrawalService : IWithdrawalService
    {
        private readonly GamingDbContext _context;
        public WithdrawalService(GamingDbContext context)
        {
            _context = context;

        }
        public async Task<WithdrawalStatsDto> GetWithdrawalStatsAsync(int agentId)
        {
            var agent = await _context.Agents.FindAsync(agentId)
                ?? throw new Exception("Agent not found");

            var withdrawals = await _context.Withdrawals
                .Where(w => w.AgentId == agentId)
                .ToListAsync();

            return new WithdrawalStatsDto
            {
                TotalRequests = withdrawals.Count,
                PendingRequests = withdrawals.Count(w => w.Status == "Pending"),
                ApprovedRequests = withdrawals.Count(w => w.Status == "Approved"),
                RejectedRequests = withdrawals.Count(w => w.Status == "Rejected"),
                TotalWithdrawn = withdrawals
                    .Where(w => w.Status == "Approved")
                    .Sum(w => w.Amount),
                PendingAmount = withdrawals
                    .Where(w => w.Status == "Pending")
                    .Sum(w => w.Amount),
                AvailableBalance = agent.Commissions.Count
            };
        }

        public async Task<PaginatedResult<WithdrawalDto>> GetWithdrawalsAsync(
            int agentId, int page, int pageSize, string? status = null)
        {
            var query = _context.Withdrawals.Where(w => w.AgentId == agentId);

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(w => w.Status == status);
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(w => w.RequestedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(w => new WithdrawalDto
                {
                    Id = w.Id,
                    Amount = w.Amount,
                    Status = w.Status,
                    RequestedDate = w.RequestedDate,
                    ProcessedDate = w.ProcessedDate,
                    Notes = w.Notes,
                    RejectionReason = w.RejectionReason
                })
                .ToListAsync();

            return new PaginatedResult<WithdrawalDto>
            {
                Items = items,
                TotalItems = total,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize)
            };

        }

        public async Task<WithdrawalDto?> CreateWithdrawalAsync(
            CreateWithdrawalDto dto, int agentId)
        {
            var agent = await _context.Agents.FindAsync(agentId)
                ?? throw new Exception("Agent not found");

            if (dto.Amount <= 0)
                throw new Exception("Amount must be greater than zero");

            if (dto.Amount > agent.Commissions.Count)
                throw new Exception("Insufficient balance");

            var hasPending = await _context.Withdrawals.AnyAsync(
                w => w.AgentId == agentId && w.Status == "Pending");

            if (hasPending)
                throw new Exception("Pending withdrawal already exists");

            var withdrawal = new Withdrawal
            {
                AgentId = agentId,
                Amount = dto.Amount,
                Notes = dto.Notes,
                Status = "Pending",
                RequestedDate = DateTime.UtcNow
            };

            _context.Withdrawals.Add(withdrawal);
            await _context.SaveChangesAsync();

            return new WithdrawalDto
            {
                Id = withdrawal.Id,
                Amount = withdrawal.Amount,
                Status = withdrawal.Status,
                RequestedDate = withdrawal.RequestedDate
            };
        }

        public async Task<bool> CancelWithdrawalAsync(int withdrawalId, int agentId)
        {
            var withdrawal = await _context.Withdrawals.FirstOrDefaultAsync(
                w => w.Id == withdrawalId &&
                     w.AgentId == agentId &&
                     w.Status == "Pending");

            if (withdrawal == null)
                return false;

            withdrawal.Status = "Rejected";
            withdrawal.ProcessedDate = DateTime.UtcNow;
            withdrawal.RejectionReason = "Cancelled by agent";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<WithdrawalDto?> GetWithdrawalByIdAsync(int withdrawalId, int agentId)
        {
            return await _context.Withdrawals
        .Where(w => w.Id == withdrawalId && w.AgentId == agentId)
        .Select(w => new WithdrawalDto
        {
            Id = w.Id,
            Amount = w.Amount,
            Status = w.Status,
            RequestedDate = w.RequestedDate,
            ProcessedDate = w.ProcessedDate,
            Notes = w.Notes,
            RejectionReason = w.RejectionReason
        })
        .FirstOrDefaultAsync();
        }
    }
    }
