using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebVacantionManager.Models
{
    public class AppUser:IdentityUser
    {
        [Required]
        [StringLength(50,MinimumLength =2)]
        public string FirstName { get; set; } = null!;
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; } = null!;
        //Navigation property
        public int? TeamId { get; set; }
        [ForeignKey(nameof(TeamId))]
        public Team? Team { get; set; }
    }
}
