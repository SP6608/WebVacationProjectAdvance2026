using System.ComponentModel.DataAnnotations;

namespace WebVacantionManager.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50,MinimumLength =2)]
        public string ProjectName { get; set; } = null!;
        [Required]
        [StringLength(4800, MinimumLength = 0)]
        public string Description { get; set; } = null!;
        //Navigation
        public ICollection<Team> Teams {  get; set; }= new HashSet<Team>();
    }
}
