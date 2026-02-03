namespace GamingPlatformAPI.DTO
{
    public class DateRangeCommissionsDto
    {
        public List<CommissionDto> Commissions { get; set; } = new List<CommissionDto>();
        public decimal TotalAmount { get; set; }
        public int TotalCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
