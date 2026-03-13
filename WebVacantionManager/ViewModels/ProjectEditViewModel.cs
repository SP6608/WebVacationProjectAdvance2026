using System.ComponentModel.DataAnnotations;

namespace WebVacantionManager.ViewModels
{
    public class ProjectEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string ProjectName { get; set; } = null!;

        [Required]
        [StringLength(4800)]
        public string Description { get; set; } = null!;
    }
}
