using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebVacantionManager.Models;
using WebVacationManager.Models;

namespace WebVacantionManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<VacationRequest> VacationRequests { get; set; } = null!;
        //public DbSet<MyRequest> MyRequests {  get; set; }   = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            builder.Entity<AppUser>()
                .HasOne(u => u.Team)
                .WithMany(t => t.Developers)
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<VacationRequest>()
                .HasOne(v => v.Applicant)
                .WithMany(u => u.VacationRequests)
                .HasForeignKey(v => v.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}