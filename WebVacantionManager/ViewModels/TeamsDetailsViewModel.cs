using WebVacantionManager.Models;

namespace WebVacantionManager.ViewModels
{
    public class TeamsDetailsViewModel
    {
        public int Id { get; set; }
        public string TeamName { get; set; } = null!;
        public string? ProjectName { get; set; }
        public string? TeamLeaderName { get; set; }
        public IList<AppUser> Developers { get; set; } =
            new List<AppUser>();
    }
}
