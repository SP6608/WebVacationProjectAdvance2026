using WebVacantionManager.Cammon;
using WebVacantionManager.Services;
using WebVacantionManager.ViewModels;

namespace WebVacantionManager.Services.Contracts
{
    public interface IVacationRequestService
    {
        Task<IEnumerable<VacationRequestIndexViewModel>> GetRequestsForUserAsync(
            string userId,
            bool isCeo,
            bool isTeamLead);

        Task<VacationRequestCreateViewModel> GetCreateModelAsync();

        Task<bool> CreateRequestAsync(
            VacationRequestCreateViewModel model,
            string userId);

        Task<VacationRequestOperationResult> ApproveRequestAsync(
            int id,
            string currentUserId,
            bool isCeo,
            bool isTeamLead);

        Task<VacationRequestDetailsViewModel?> GetDetailsAsync(
            int id,
            string currentUserId,
            bool isCeo,
            bool isTeamLead);

        Task<VacationRequestEditViewModel?> GetEditModelAsync(
            int id,
            string currentUserId);

        Task<VacationRequestOperationResult> EditRequestAsync(
            VacationRequestEditViewModel model,
            string currentUserId);

        Task<VacationRequestDeleteViewModel?> GetDeleteModelAsync(
            int id,
            string currentUserId);

        Task<VacationRequestOperationResult> DeleteRequestAsync(
            int id,
            string currentUserId);

        Task<bool> ExistsAsync(int id);
    }
}