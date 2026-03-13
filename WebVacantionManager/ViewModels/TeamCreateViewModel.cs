using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebVacantionManager.ViewModels
{
    public class TeamCreateViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string TeamName { get; set; } = null!;

        public int? ProjectId { get; set; }

        public string? TeamLeaderId { get; set; }

        public IEnumerable<SelectListItem> Projects { get; set; } =
            new List<SelectListItem>();

        public IEnumerable<SelectListItem> TeamLeaders { get; set; } =
            new List<SelectListItem>();
    }
}