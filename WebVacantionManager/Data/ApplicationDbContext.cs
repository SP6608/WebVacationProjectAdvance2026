using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebVacantionManager.Models;

namespace WebVacantionManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            builder.Entity<AppUser>()
                .HasOne(u => u.Team)
                .WithMany(t => t.Developers)
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.SetNull);
        }
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;
        
    }
}
