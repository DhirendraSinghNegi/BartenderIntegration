using BartenderIntegration.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BartenderIntegration.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserProfile>(entity =>
            {
                entity.HasOne(p => p.AppUser)
                .WithOne(u => u.UserProfile)
                .HasForeignKey<UserProfile>(fk => fk.UserId).
                OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
