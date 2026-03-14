using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebVacantionManager.Cammon
{
    public class UsersRolesTypeConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        private readonly ICollection<IdentityUserRole<string>> UsersRoles =
            new HashSet<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>()
                {
                    UserId = "1f1a1111-1111-1111-1111-111111111111",
                    RoleId = "3a310333-c4a5-4457-9065-a861e635d848" // Ceo
                },

                new IdentityUserRole<string>()
                {
                    UserId = "2f2b2222-2222-2222-2222-222222222222",
                    RoleId = "dd504816-18f8-420f-a69f-ca3e6386ba5c" // TeamLead
                },

                new IdentityUserRole<string>()
                {
                    UserId = "3f3c3333-3333-3333-3333-333333333333",
                    RoleId = "96554597-d11d-48bb-84d5-bbf7442a7afc" // Developer
                }
            };

        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(UsersRoles);
        }
    }
}