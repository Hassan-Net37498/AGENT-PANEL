namespace GamingPlatformAPI.DTO
{
    public class MonthlyEarningsDto
    {
        public string Month { get; set; } = string.Empty; // e.g., "2025-01"
        public decimal TotalEarnings { get; set; }
        public int TransactionCount { get; set; }
    }
}
