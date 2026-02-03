namespace GamingPlatformAPI.DTO
{
    public class CommissionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal CommissionRate { get; set; }
        public DateTime EarnedDate { get; set; }
        public bool IsPaid { get; set; }
    }
}
