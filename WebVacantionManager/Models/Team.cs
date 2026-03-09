using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebVacantionManager.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50,MinimumLength =2)]
        public string TeamName { get; set; } = null!;

        public string? TeamLeaderId { get; set; }
        [ForeignKey(nameof(TeamLeaderId))]
        public AppUser TeamLeader { get; set; } = null!;
        //Navigation
        public int? ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project? Project { get; set; }
        public ICollection<AppUser> Developers { get; set; }= new HashSet<AppUser>();
    }
}
