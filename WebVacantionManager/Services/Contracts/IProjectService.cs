using WebVacantionManager.ViewModels;

namespace WebVacantionManager.Services.Contracts
{
    public interface IProjectService
    {
        Task<ICollection<ProjectDetailsViewModel>> GetAllAsync();
        Task<ProjectDetailsViewModel?> GetByIdAsync(int id);
        Task CreateAsync(ProjectsCreateViewModel model);
        Task<ProjectEditViewModel?> GetForEditAsync(int id);
        Task<bool> EditAsync(ProjectEditViewModel model);
        Task<ProjectDeleteViewModel?> GetForDeleteAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}