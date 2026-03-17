using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebVacantionManager.Services.Contracts;
using WebVacantionManager.ViewModels;

namespace WebVacantionManager.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IProjectService projectService;

        public ProjectsController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ICollection<ProjectDetailsViewModel> models = await projectService.GetAllAsync();

            return View(models);
        }

        [HttpGet]
        [Authorize(Roles = "Ceo")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Ceo")]
        public async Task<IActionResult> Create(ProjectsCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await projectService.CreateAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            ProjectDetailsViewModel? project = await projectService.GetByIdAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [HttpGet]
        [Authorize(Roles = "Ceo")]
        public async Task<IActionResult> Edit(int id)
        {
            ProjectEditViewModel? model = await projectService.GetForEditAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Ceo")]
        public async Task<IActionResult> Edit(ProjectEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isEdited = await projectService.EditAsync(model);

            if (!isEdited)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Ceo")]
        public async Task<IActionResult> Delete(int id)
        {
            ProjectDeleteViewModel? model = await projectService.GetForDeleteAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Ceo")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool isDeleted = await projectService.DeleteAsync(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}