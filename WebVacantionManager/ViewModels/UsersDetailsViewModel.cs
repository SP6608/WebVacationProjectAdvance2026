namespace WebVacantionManager.ViewModels
{
    public class UsersDetailsViewModel
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? RoleName { get; set; }
        public string? TeamName { get; set; }
        public int VacationRequestsCount { get; set; }
    }
}
