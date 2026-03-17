using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebVacantionManager.Cammon;

using WebVacantionManager.Services.Contracts;
using WebVacantionManager.ViewModels;

namespace WebVacantionManager.Controllers
{
    [Authorize]
    public class VacationRequestsController : Controller
    {
        private readonly IVacationRequestService vacationRequestService;

        public VacationRequestsController(IVacationRequestService vacationRequestService)
        {
            this.vacationRequestService = vacationRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            bool isCeo = User.IsInRole("Ceo");
            bool isTeamLead = User.IsInRole("TeamLead");

            IEnumerable<VacationRequestIndexViewModel> model =
                await vacationRequestService.GetRequestsForUserAsync(currentUserId, isCeo, isTeamLead);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            VacationRequestCreateViewModel model =
                await vacationRequestService.GetCreateModelAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VacationRequestCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.VacationTypes = (await vacationRequestService.GetCreateModelAsync()).VacationTypes;
                return View(model);
            }

            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            bool success = await vacationRequestService.CreateRequestAsync(model, currentUserId);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Ceo,TeamLead")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            bool isCeo = User.IsInRole("Ceo");
            bool isTeamLead = User.IsInRole("TeamLead");

            VacationRequestOperationResult result =
                await vacationRequestService.ApproveRequestAsync(id, currentUserId, isCeo, isTeamLead);

            if (result == VacationRequestOperationResult.NotFound)
            {
                return NotFound();
            }

            if (result == VacationRequestOperationResult.Forbidden)
            {
                return Forbid();
            }

            if (result == VacationRequestOperationResult.AlreadyProcessed)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            bool isCeo = User.IsInRole("Ceo");
            bool isTeamLead = User.IsInRole("TeamLead");

            VacationRequestDetailsViewModel? model =
                await vacationRequestService.GetDetailsAsync(id, currentUserId, isCeo, isTeamLead);

            if (model == null)
            {
                bool exists = await vacationRequestService.ExistsAsync(id);

                if (!exists)
                {
                    return NotFound();
                }

                return Forbid();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            VacationRequestEditViewModel? model =
                await vacationRequestService.GetEditModelAsync(id, currentUserId);

            if (model == null)
            {
                bool exists = await vacationRequestService.ExistsAsync(id);

                if (!exists)
                {
                    return NotFound();
                }

                return Forbid();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VacationRequestEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.VacationTypes = (await vacationRequestService.GetCreateModelAsync()).VacationTypes;
                return View(model);
            }

            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            VacationRequestOperationResult result =
                await vacationRequestService.EditRequestAsync(model, currentUserId);

            if (result == VacationRequestOperationResult.NotFound)
            {
                return NotFound();
            }

            if (result == VacationRequestOperationResult.Forbidden ||
                result == VacationRequestOperationResult.AlreadyProcessed)
            {
                return Forbid();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            VacationRequestDeleteViewModel? model =
                await vacationRequestService.GetDeleteModelAsync(id, currentUserId);

            if (model == null)
            {
                bool exists = await vacationRequestService.ExistsAsync(id);

                if (!exists)
                {
                    return NotFound();
                }

                return Forbid();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            VacationRequestOperationResult result =
                await vacationRequestService.DeleteRequestAsync(id, currentUserId);

            if (result == VacationRequestOperationResult.NotFound)
            {
                return NotFound();
            }

            if (result == VacationRequestOperationResult.Forbidden ||
                result == VacationRequestOperationResult.AlreadyProcessed)
            {
                return Forbid();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}