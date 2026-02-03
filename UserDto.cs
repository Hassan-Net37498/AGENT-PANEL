namespace GamingPlatformAPI.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Country { get; set; }
        public decimal TotalDeposits { get; set; }
        public decimal TotalWagers { get; set; }
        public decimal TotalLosses { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
