using System.ComponentModel.DataAnnotations;

namespace GamingPlatformAPI.models
{
    public class Withdrawal
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty; // Pending, Approved, Rejected
        public DateTime RequestedDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string? Notes { get; set; }
        public string? RejectionReason { get; set; }
        public int AgentId { get; set; }
        public Agent Agent { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
