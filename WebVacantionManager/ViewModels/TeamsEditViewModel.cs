using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebVacantionManager.ViewModels
{
    public class TeamsEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Team Name")]
        public string TeamName { get; set; } = null!;

        [Display(Name = "Project")]
        public int? ProjectId { get; set; }

        [Display(Name = "Team Leader")]
        public string? TeamLeaderId { get; set; }

        public IEnumerable<SelectListItem> Projects { get; set; } =
            new List<SelectListItem>();

        public IEnumerable<SelectListItem> TeamLeaders { get; set; } =
            new List<SelectListItem>();
    }
}
