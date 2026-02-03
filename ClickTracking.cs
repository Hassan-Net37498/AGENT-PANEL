using System.ComponentModel.DataAnnotations;

namespace GamingPlatformAPI.models
{
    public class ClickTracking
    {
        [Key]
        public int Id { get; set; }
        public int AffiliateId { get; set; }
        public Affiliate Affiliate { get; set; }
        public string IPAddress { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
