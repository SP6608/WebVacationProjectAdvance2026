using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebVacantionManager.ViewModels
{
    public class UsersEditViewModel
    {
        public string Id { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; } = null!;
        public int? TeamId { get; set; }
        public string? RoleName { get; set; }
        public IEnumerable<SelectListItem> Teams { get; set; }
            = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Roles { get; set; }
            = new List<SelectListItem>();
    }
}
