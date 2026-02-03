using System.ComponentModel.DataAnnotations;

namespace GamingPlatformAPI.models
{
    public class Agent
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<user> Users { get; set; }
        public ICollection<Commission> Commissions { get; set; }
    }
}
