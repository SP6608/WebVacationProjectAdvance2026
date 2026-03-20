using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebVacationManager.Models;
using WebVacantionManager.Cammon;
using WebVacantionManager.Data;
using WebVacantionManager.Services.Contracts;
using WebVacantionManager.ViewModels;

namespace WebVacantionManager.Services
{
    public class VacationRequestService : IVacationRequestService
    {
        private readonly ApplicationDbContext context;

        public VacationRequestService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<VacationRequestIndexViewModel>> GetRequestsForUserAsync(
            string userId,
            bool isCeo,
            bool isTeamLead)
        {
            IQueryable<VacationRequest> query = context.VacationRequests
                .AsNoTracking()
                .Include(vr => vr.Applicant);

            if (!isCeo && !isTeamLead)
            {
                query = query.Where(vr => vr.ApplicantId == userId);
            }

            return await query
                .OrderByDescending(vr => vr.CreatedOn)
                .Select(vr => new VacationRequestIndexViewModel
                {
                    Id = vr.Id,
                    ApplicantName = vr.Applicant.UserName!,
                    DateFrom = vr.DateFrom,
                    DateTo = vr.DateTo,
                    CreatedOn = vr.CreatedOn,
                    IsHalfDay = vr.IsHalfDay,
                    Status = vr.Status.ToString(),
                    VacationType = vr.VacationType.ToString()
                })
                .ToListAsync();
        }

        public Task<VacationRequestCreateViewModel> GetCreateModelAsync()
        {
            VacationRequestCreateViewModel model = new VacationRequestCreateViewModel
            {
                VacationTypes = GetVacationTypes()
            };

            return Task.FromResult(model);
        }

        public async Task<bool> CreateRequestAsync(
            VacationRequestCreateViewModel model,
            string userId)
        {
            if (model.DateTo < model.DateFrom)
            {
                return false;
            }

            VacationRequest request = new VacationRequest
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo,
                IsHalfDay = model.IsHalfDay,
                VacationType = model.VacationType,
                ApplicantId = userId,
                CreatedOn = DateTime.UtcNow,
                Status = RequestStatus.Pending
            };

            await context.VacationRequests.AddAsync(request);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<VacationRequestOperationResult> ApproveRequestAsync(
            int id,
            string currentUserId,
            bool isCeo,
            bool isTeamLead)
        {
            VacationRequest? request = await context.VacationRequests
                .Include(vr => vr.Applicant)
                .FirstOrDefaultAsync(vr => vr.Id == id);

            if (request == null)
            {
                return VacationRequestOperationResult.NotFound;
            }

            if (!isCeo && !isTeamLead)
            {
                return VacationRequestOperationResult.Forbidden;
            }

            if (request.Status != RequestStatus.Pending)
            {
                return VacationRequestOperationResult.AlreadyProcessed;
            }

            request.Status = RequestStatus.Approved;
            await context.SaveChangesAsync();

            return VacationRequestOperationResult.Success;
        }

        public async Task<VacationRequestDetailsViewModel?> GetDetailsAsync(
            int id,
            string currentUserId,
            bool isCeo,
            bool isTeamLead)
        {
            VacationRequest? request = await context.VacationRequests
                .AsNoTracking()
                .Include(vr => vr.Applicant)
                .FirstOrDefaultAsync(vr => vr.Id == id);

            if (request == null)
            {
                return null;
            }

            bool hasAccess = request.ApplicantId == currentUserId || isCeo || isTeamLead;

            if (!hasAccess)
            {
                return null;
            }

            return new VacationRequestDetailsViewModel
            {
                Id = request.Id,
                ApplicantName = request.Applicant.UserName!,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo,
                CreatedOn = request.CreatedOn,
                IsHalfDay = request.IsHalfDay,
                Status = request.Status.ToString(),
                VacationType = request.VacationType.ToString()
            };
        }

        public async Task<VacationRequestEditViewModel?> GetEditModelAsync(
            int id,
            string currentUserId)
        {
            VacationRequest? request = await context.VacationRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(vr => vr.Id == id);

            if (request == null)
            {
                return null;
            }

            if (request.ApplicantId != currentUserId || request.Status != RequestStatus.Pending)
            {
                return null;
            }

            return new VacationRequestEditViewModel
            {
                Id = request.Id,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo,
                IsHalfDay = request.IsHalfDay,
                VacationType = request.VacationType,
                VacationTypes = GetVacationTypes()
            };
        }

        public async Task<VacationRequestOperationResult> EditRequestAsync(
            VacationRequestEditViewModel model,
            string currentUserId)
        {
            VacationRequest? request = await context.VacationRequests
                .FirstOrDefaultAsync(vr => vr.Id == model.Id);

            if (request == null)
            {
                return VacationRequestOperationResult.NotFound;
            }

            if (request.ApplicantId != currentUserId)
            {
                return VacationRequestOperationResult.Forbidden;
            }

            if (request.Status != RequestStatus.Pending)
            {
                return VacationRequestOperationResult.AlreadyProcessed;
            }

            if (model.DateTo < model.DateFrom)
            {
                return VacationRequestOperationResult.Forbidden;
            }

            request.DateFrom = model.DateFrom;
            request.DateTo = model.DateTo;
            request.IsHalfDay = model.IsHalfDay;
            request.VacationType = model.VacationType;

            await context.SaveChangesAsync();

            return VacationRequestOperationResult.Success;
        }

        public async Task<VacationRequestDeleteViewModel?> GetDeleteModelAsync(
            int id,
            string currentUserId)
        {
            VacationRequest? request = await context.VacationRequests
                .AsNoTracking()
                .Include(vr => vr.Applicant)
                .FirstOrDefaultAsync(vr => vr.Id == id);

            if (request == null)
            {
                return null;
            }

            if (request.ApplicantId != currentUserId || request.Status != RequestStatus.Pending)
            {
                return null;
            }

            return new VacationRequestDeleteViewModel
            {
                Id = request.Id,
                ApplicantName = request.Applicant.UserName!,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo,
                CreatedOn = request.CreatedOn,
                IsHalfDay = request.IsHalfDay,
                Status = request.Status.ToString(),
                VacationType = request.VacationType.ToString()
            };
        }

        public async Task<VacationRequestOperationResult> DeleteRequestAsync(
     int id,
     string currentUserId,
     bool isCeo,
     bool isTeamLead)
        {
            var request = await context.VacationRequests
                .FirstOrDefaultAsync(vr => vr.Id == id);

            if (request == null)
            {
                return VacationRequestOperationResult.NotFound;
            }

            
            if (isCeo)
            {
                
            }
            
            else if (isTeamLead)
            {
                bool isInTeam = await IsUserInTeamOfLeader(request.ApplicantId, currentUserId);

                if (!isInTeam)
                {
                    return VacationRequestOperationResult.Forbidden;
                }
            }
            
            else
            {
                if (request.ApplicantId != currentUserId)
                {
                    return VacationRequestOperationResult.Forbidden;
                }
            }

            
            if (request.Status != RequestStatus.Pending)
            {
                return VacationRequestOperationResult.AlreadyProcessed;
            }

            context.VacationRequests.Remove(request);
            await context.SaveChangesAsync();

            return VacationRequestOperationResult.Success;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.VacationRequests.AnyAsync(vr => vr.Id == id);
        }

        private List<SelectListItem> GetVacationTypes()
        {
            return Enum.GetValues(typeof(VacationType))
                .Cast<VacationType>()
                .Select(vt => new SelectListItem
                {
                    Value = ((int)vt).ToString(),
                    Text = vt.ToString()
                })
                .ToList();
        }
        private async Task<bool> IsUserInTeamOfLeader(string applicantId, string leaderId)
        {
            return await context.Teams
                .AnyAsync(t =>
                    t.TeamLeaderId == leaderId &&
                    t.Developers.Any(d => d.Id == applicantId));
        }
    }
}