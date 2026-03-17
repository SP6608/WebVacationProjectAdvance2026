using WebVacantionManager.ViewModels;

namespace WebVacantionManager.Services.Contracts
{
    public interface ITeamService
    {
        Task<ICollection<TeamIndexViewModel>> GetAllAsync();
        Task<TeamsDetailsViewModel?> GetByIdAsync(int id);
        Task<TeamCreateViewModel> GetCreateModelAsync();
        Task CreateAsync(TeamCreateViewModel model);
        Task<TeamsEditViewModel?> GetForEditAsync(int id);
        Task<bool> EditAsync(TeamsEditViewModel model);
        Task<TeamsDeleteViewModel?> GetForDeleteAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task PopulateCreateModelAsync(TeamCreateViewModel model);
        Task PopulateEditModelAsync(TeamsEditViewModel model);
    }
}