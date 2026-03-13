using System.ComponentModel.DataAnnotations;

namespace WebVacantionManager.ViewModels
{
    public class ProjectDeleteViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        public IEnumerable<TeamSimpleViewModel> Teams { get; set; } =
            new List<TeamSimpleViewModel>();
    }
}
