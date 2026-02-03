namespace GamingPlatformAPI.DTO
{
    public class WeeklyEarningsDto
    {
        public List<EarningsChartDto> Data { get; set; } = new List<EarningsChartDto>();
        public decimal TotalEarnings { get; set; }
        public decimal AverageDaily { get; set; }
    }
}
