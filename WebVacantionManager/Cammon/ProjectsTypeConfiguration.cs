using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebVacantionManager.Models;

namespace WebVacantionManager.Cammon
{
    public class ProjectsTypeConfiguration : IEntityTypeConfiguration<Project>
    {
        private ICollection<Project> Projects=
            new HashSet<Project>() 
            {
                 new Project
    {
        Id=3, 
        ProjectName = "School Management System",
        Description = "System for managing students, teachers and courses."
    },
    new Project
    {   Id=4,
        ProjectName = "Task Tracker",
        Description = "Application for tracking tasks, deadlines and productivity."
    },
    new Project
    {  Id =5,
        ProjectName = "Restaurant Reservation System",
        Description = "Web platform for booking tables and managing reservations."
    },
    new Project
    {     
        Id=6,
        ProjectName = "Fitness Tracker",
        Description = "Application that tracks workouts, calories and progress."
    }
            };
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasData(Projects);
        }
    }
}
