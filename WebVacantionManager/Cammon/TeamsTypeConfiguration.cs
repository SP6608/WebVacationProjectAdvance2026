using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebVacantionManager.Models;

namespace WebVacantionManager.Cammon
{
    public class TeamsTypeConfiguration : IEntityTypeConfiguration<Team>
    {
        private readonly ICollection<Team> Teams = new HashSet<Team>()
        {
            new Team()
            {
                Id = 1,
                TeamName = "Backend Team",
                ProjectId = 3,
                TeamLeaderId = "2f2b2222-2222-2222-2222-222222222222"
            },

            new Team()
            {
                Id = 2,
                TeamName = "Frontend Team",
                ProjectId = 4,
                TeamLeaderId = "2f2b2222-2222-2222-2222-222222222222"
            },

            new Team()
            {
                Id = 3,
                TeamName = "Mobile Team",
                ProjectId = 5,
                TeamLeaderId = null
            } 
        };
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasData(Teams);
        }
    }
}
