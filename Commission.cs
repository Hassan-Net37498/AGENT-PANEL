using System.ComponentModel.DataAnnotations;

namespace GamingPlatformAPI.models
{
    public class Commission
    {
        [Key]
        public int Id { get; set; }
        public int AgentId { get; set; }
        public Agent Agent { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public decimal BaseAmount { get; set; }
        public decimal CommissionRate { get; set; }
        public user user { get; set; }
        public DateTime EarnedDate { get; set; }
        public bool IsPaid { get; set; }
        public DateTime Date { get; set; }
    }
}
