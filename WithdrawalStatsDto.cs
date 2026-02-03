namespace GamingPlatformAPI.DTO
{
    public class WithdrawalStatsDto
    {
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public decimal TotalWithdrawn { get; set; }
        public decimal PendingAmount { get; set; }
        public decimal AvailableBalance { get; set; }
    }
}
