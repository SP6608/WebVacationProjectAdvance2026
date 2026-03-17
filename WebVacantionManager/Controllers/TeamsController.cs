using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebVacantionManager.Services.Contracts;
using WebVacantionManager.ViewModels;

namespace WebVacantionManager.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ITeamService teamService;

        public TeamsController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ICollection<TeamIndexViewModel> model = await teamService.GetAllAsync();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Ceo")]
        public async Task<IActionResult> Create()
        {
            TeamCreateViewModel model = await teamService.GetCreateModelAsync();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Ceo")]
        public async Task<IActionResult> Create(TeamCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await teamService.PopulateCreateModelAsync(model);
                return View(model);
            }

            await teamService.CreateAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            TeamsDetailsViewModel? model = await teamService.GetByIdAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Ceo")]
        public async Task<IActionResult> Edit(int id)
        {
            TeamsEditViewModel? model = await teamService.GetForEditAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Ceo")]
        public async Task<IActionResult> Edit(TeamsEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await teamService.PopulateEditModelAsync(model);
                return View(model);
            }

            bool isEdited = await teamService.EditAsync(model);

            if (!isEdited)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Ceo")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            TeamsDeleteViewModel? model = await teamService.GetForDeleteAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Ceo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool isDeleted = await teamService.DeleteAsync(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}