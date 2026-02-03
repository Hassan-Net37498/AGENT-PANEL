using GamingPlatformAPI.DTO;

namespace GamingPlatformAPI.iService
{
    public interface IWithdrawalService
    {
        Task<WithdrawalStatsDto> GetWithdrawalStatsAsync(int agentId);
        Task<PaginatedResult<WithdrawalDto>> GetWithdrawalsAsync(int agentId, int page, int pageSize, string? status = null);
        Task<WithdrawalDto?> GetWithdrawalByIdAsync(int withdrawalId, int agentId);
        Task<WithdrawalDto?> CreateWithdrawalAsync(CreateWithdrawalDto createDto, int agentId);
        Task<bool> CancelWithdrawalAsync(int withdrawalId, int agentId);
    }
}
