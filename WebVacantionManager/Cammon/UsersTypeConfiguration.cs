using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebVacantionManager.Models;

namespace WebVacantionManager.Cammon
{
    public class UsersTypeConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            PasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();

            AppUser ceo = new AppUser
            {
                Id = "1f1a1111-1111-1111-1111-111111111111",
                UserName = "ceo@company.com",
                NormalizedUserName = "CEO@COMPANY.COM",
                Email = "ceo@company.com",
                NormalizedEmail = "CEO@COMPANY.COM",
                FirstName = "Ivan",
                LastName = "Petrov",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            AppUser teamLead = new AppUser
            {
                Id = "2f2b2222-2222-2222-2222-222222222222",
                UserName = "teamlead@company.com",
                NormalizedUserName = "TEAMLEAD@COMPANY.COM",
                Email = "teamlead@company.com",
                NormalizedEmail = "TEAMLEAD@COMPANY.COM",
                FirstName = "Maria",
                LastName = "Ivanova",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                TeamId = 1
            };

            AppUser developer = new AppUser
            {
                Id = "3f3c3333-3333-3333-3333-333333333333",
                UserName = "developer@company.com",
                NormalizedUserName = "DEVELOPER@COMPANY.COM",
                Email = "developer@company.com",
                NormalizedEmail = "DEVELOPER@COMPANY.COM",
                FirstName = "Georgi",
                LastName = "Dimitrov",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                TeamId = 1
            };

            ceo.PasswordHash = passwordHasher.HashPassword(ceo, "123456");
            teamLead.PasswordHash = passwordHasher.HashPassword(teamLead, "123456");
            developer.PasswordHash = passwordHasher.HashPassword(developer, "123456");

            builder.HasData(ceo, teamLead, developer);
        }
    }
}