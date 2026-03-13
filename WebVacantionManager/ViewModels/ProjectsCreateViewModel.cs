using System.ComponentModel.DataAnnotations;

namespace WebVacantionManager.ViewModels
{
    public class ProjectsCreateViewModel
    {
        [Required]
        [StringLength(50,MinimumLength =2)]
        public string Projectname { get; set; } = null!;
        [Required]
        [MaxLength(4080)]
        public string Description { get; set; }= null!;
    }
}
