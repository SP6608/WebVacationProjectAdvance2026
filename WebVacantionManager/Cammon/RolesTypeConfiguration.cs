using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebVacantionManager.Cammon
{
    public class RolesTypeConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        private readonly ICollection<IdentityRole> roles =
            new List<IdentityRole>()
            {
                new IdentityRole
                {
                    Id = "b4d5e8fa-7d76-4ff9-95f1-1f1f3a52c111",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                },
                new IdentityRole
                {
                    Id = "3a310333-c4a5-4457-9065-a861e635d848",
                    Name = "Ceo",
                    NormalizedName = "CEO"
                },
                new IdentityRole
                {
                    Id = "96554597-d11d-48bb-84d5-bbf7442a7afc",
                    Name = "Developer",
                    NormalizedName = "DEVELOPER"
                },
                new IdentityRole
                {
                    Id = "e3233cd7-132a-45ca-8a32-7160411726ed",
                    Name = "Unassigned",
                    NormalizedName = "UNASSIGNED"
                },
                new IdentityRole
                {
                    Id = "dd504816-18f8-420f-a69f-ca3e6386ba5c",
                    Name = "TeamLead",
                    NormalizedName = "TEAMLEAD"
                }
            };

        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(roles);
        }
    }
}