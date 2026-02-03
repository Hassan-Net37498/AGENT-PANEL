using GamingPlatformAPI.models;
using GamingPlatformAPI.ORM;

namespace GamingPlatformAPI.seeddata
{
    public static class DbInitializer
    {
        public static void Initialize(GamingDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Agents.Any()) return;

            var agent1 = new Agent { Name = "Agent A", Email = "agentA@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };
            var agent2 = new Agent { Name = "Agent B", Email = "agentB@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };
            context.Agents.AddRange(agent1, agent2);

            var affiliate1 = new Affiliate { Name = "Affiliate A", Email = "affA@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };
            var affiliate2 = new Affiliate { Name = "Affiliate B", Email = "affB@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };
            context.Affiliates.AddRange(affiliate1, affiliate2);

            for (int i = 1; i <= 5; i++)
            {
                context.Users.Add(new user
                {
                    FullName = $"User {i}",
                    Email = $"user{i}@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                    Agent = agent1,
                    IsBlocked = false
                });
            }

            context.SaveChanges();
        }
    }
}
