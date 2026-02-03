namespace GamingPlatformAPI.DTO
{
    public class EarningsChartDto
    {
        public string Date { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Label { get; set; } = string.Empty; 
    }
}
