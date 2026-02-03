using GamingPlatformAPI.models;
using Microsoft.EntityFrameworkCore;

namespace GamingPlatformAPI.ORM
{
    public class GamingDbContext:DbContext
    {
        public GamingDbContext(DbContextOptions<GamingDbContext> options):base(options)
        {
            
        }
        public DbSet<user> Users { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Affiliate> Affiliates { get; set; }
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<Withdrawal> Withdrawals { get; set; }
        public DbSet<ClickTracking> Clicks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<user>().HasIndex(o => o.Email).IsUnique();
            modelBuilder.Entity<Agent>().HasIndex(o=> o.Email).IsUnique();
            modelBuilder.Entity<Affiliate>().HasIndex (o=> o.Email).IsUnique();
            modelBuilder.Entity<Agent>().HasData(
        new Agent { Id=1,  Name = "John Doe", Email = "john@example.com", PasswordHash = "hashedpassword" }
    );
            foreach (var fk in modelBuilder.Model.GetEntityTypes()
        .SelectMany(e => e.GetForeignKeys()))
            {
                fk.DeleteBehavior = DeleteBehavior.NoAction;
            }
            // Seed User one by one
            modelBuilder.Entity<user>().HasData(
        new user
        {
            Id = 1,
            FullName = "Alice Johnson",
            Email = "alice@example.com",
            PasswordHash = "hashedpassword",
            IsBlocked = false,
            PhoneNumber = "1234567890",
            CreatedAt = new DateTime(2026, 1, 1), // STATIC DATE
            Country = "USA",
            TotalDeposits = 1000.00m,
            TotalWagers = 500.00m,
            TotalLosses = 100.00m,
            AgentId = 1
        },
        new user
        {
            Id = 2,
            FullName = "Bob Smith",
            Email = "bob@example.com",
            PasswordHash = "hashedpassword",
            IsBlocked = false,
            PhoneNumber = "0987654321",
            CreatedAt = new DateTime(2026, 1, 2), // STATIC DATE
            Country = "USA",
            TotalDeposits = 2000.00m,
            TotalWagers = 1500.00m,
            TotalLosses = 200.00m,
            AgentId = 1
        },
         new user
         {
             Id = 3,
             FullName = "Bob Smith",
             Email = "bob@example.com",
             PasswordHash = "hashedpassword",
             IsBlocked = false,
             PhoneNumber = "0987654321",
             CreatedAt = new DateTime(2026, 1, 2), // STATIC DATE
             Country = "USA",
             TotalDeposits = 2000.00m,
             TotalWagers = 1500.00m,
             TotalLosses = 200.00m,
             AgentId = 1
         },
          new user
          {
              Id = 4,
              FullName = "Bob Smith",
              Email = "bob@example.com",
              PasswordHash = "hashedpassword",
              IsBlocked = false,
              PhoneNumber = "0987654321",
              CreatedAt = new DateTime(2026, 1, 2), // STATIC DATE
              Country = "USA",
              TotalDeposits = 2000.00m,
              TotalWagers = 1500.00m,
              TotalLosses = 200.00m,
              AgentId = 1
          }
    );
            modelBuilder.Entity<Commission>()
     .Property(x => x.BaseAmount)
     .HasPrecision(18, 4);

            modelBuilder.Entity<Commission>()
                .Property(x => x.CommissionRate)
                .HasPrecision(5, 4);

            modelBuilder.Entity<user>()
                .Property(x => x.TotalDeposits)
                .HasPrecision(18, 2);

            modelBuilder.Entity<user>()
                .Property(x => x.TotalLosses)
                .HasPrecision(18, 2);

            modelBuilder.Entity<user>()
                .Property(x => x.TotalWagers)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Commission>().HasData(
               new Commission { Id = 1, AgentId = 1, Amount = 100.50m, Date = new DateTime(2026, 1, 31, 12, 0, 0) },
               new Commission { Id = 2, AgentId = 1, Amount = 200.75m, Date = new DateTime(2026, 1, 30, 12, 0, 0) },
               new Commission { Id = 3, AgentId = 1, Amount = 150.00m, Date = new DateTime(2026, 1, 29, 12, 0, 0) }
           );
            modelBuilder.Entity<ClickTracking>().HasData(
                         new ClickTracking { Id = 1, AffiliateId = 1, IPAddress = "192.168.1.100", Timestamp = new DateTime(2026, 1, 31, 12, 0, 0) },
                         new ClickTracking { Id = 2, AffiliateId = 1, IPAddress = "192.168.1.101", Timestamp = new DateTime(2026, 1, 31, 12, 5, 0) }
                     );

            // Seed Withdrawals
            modelBuilder.Entity<Withdrawal>().HasData(
                new Withdrawal { Id = 1, AgentId = 1, Amount = 50.00m, Status = "Pending", RequestDate = new DateTime(2026, 1, 31, 12, 0, 0) },
                new Withdrawal { Id = 2, AgentId = 1, Amount = 100.00m, Status = "Approved", RequestDate = new DateTime(2026, 1, 30, 12, 0, 0) }
            );
            modelBuilder.Entity<Affiliate>().HasData(
       new Affiliate { Id = 1, Name = "SuperAffiliate", Email = "affiliate@example.com", PasswordHash = "hashedpassword" }
   );

         
            modelBuilder.Entity<Commission>()
       .Property(c => c.Amount)
       .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Withdrawal>()
                .Property(w => w.Amount)
                .HasColumnType("decimal(18,2)");
        }

    }

}
