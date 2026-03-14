namespace WebVacantionManager.ViewModels
{
    public class UsersIndexViewModel
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? RoleName { get; set; }
        public string? TeamName { get; set; }
    }
}
