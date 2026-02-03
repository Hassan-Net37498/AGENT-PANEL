namespace GamingPlatformAPI.DTO
{
    public class WithdrawalDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty; // Pending, Approved, Rejected
        public DateTime RequestedDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string? Notes { get; set; }
        public string? RejectionReason { get; set; }
    }
}
