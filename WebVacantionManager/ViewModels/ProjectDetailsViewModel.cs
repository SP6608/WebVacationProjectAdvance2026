using WebVacantionManager.Models;

namespace WebVacantionManager.ViewModels
{
    public class ProjectDetailsViewModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ICollection<Team>Teams { get; set; }=new List<Team>();
    }
}
