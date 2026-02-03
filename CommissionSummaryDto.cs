namespace GamingPlatformAPI.DTO
{
    public class CommissionSummaryDto
    {
        public decimal TotalEarned { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalPending { get; set; }
        public int TotalTransactions { get; set; }
        public decimal AverageCommission { get; set; }
        public decimal ThisMonthEarnings { get; set; }
        public decimal LastMonthEarnings { get; set; }
    }
}
