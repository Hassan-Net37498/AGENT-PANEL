namespace GamingPlatformAPI.DTO
{
    public class DashboardDto
    {
        public int TotalUsers { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal PendingCommission { get; set; }
        public decimal WithdrawableBalance { get; set; }
        public int ActiveUsers { get; set; }
        public int BlockedUsers { get; set; }
    }
}
