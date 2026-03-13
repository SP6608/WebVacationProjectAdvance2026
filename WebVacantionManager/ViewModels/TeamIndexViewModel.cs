namespace WebVacantionManager.ViewModels
{
    public class TeamIndexViewModel
    {
        public int Id { get; set; }
        public string TeamName { get; set; } = null!;
        public string? ProjectName { get; set; }
        public string? TeamLeaderName { get; set; }
        public int DevelopersCount { get; set; }
    }
}
