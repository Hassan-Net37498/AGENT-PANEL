using System.ComponentModel.DataAnnotations;

namespace GamingPlatformAPI.models
{
    public class user
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsBlocked { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Country { get; set; }
        public decimal TotalDeposits { get; set; }
        public decimal TotalWagers { get; set; }
        public decimal TotalLosses { get; set; }
        public int AgentId { get; set; }
        public Agent Agent { get; set; }
    }
}
