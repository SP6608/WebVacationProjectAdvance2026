using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebVacantionManager.Models;

namespace WebVacantionManager.Cammon
{
    public class TeamsTypeConfiguration : IEntityTypeConfiguration<Team>
    {
        private readonly ICollection<Team> teams =
            new HashSet<Team>()
            {
                new Team
                {
                    Id = 1,
                    TeamName = "Backend Team",
                    ProjectId = 3,
                    TeamLeaderId = null
                },
                new Team
                {
                    Id = 2,
                    TeamName = "Frontend Team",
                    ProjectId = 4,
                    TeamLeaderId = null
                },
                new Team
                {
                    Id = 3,
                    TeamName = "Mobile Team",
                    ProjectId = 5,
                    TeamLeaderId = null
                }
            };

        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasData(teams);
        }
    }
}