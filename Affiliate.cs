using System.ComponentModel.DataAnnotations;

namespace GamingPlatformAPI.models
{
    public class Affiliate
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<ClickTracking> Clicks { get; set; }
    }
}
