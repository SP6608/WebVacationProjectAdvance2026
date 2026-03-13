using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebVacantionManager.Data;
using WebVacantionManager.Models;
using WebVacantionManager.ViewModels;

namespace WebVacantionManager.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext context;
        public ProjectsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ICollection<ProjectDetailsViewModel> models =
                 context
                .Projects
                .AsNoTracking()
                .Select(p => new ProjectDetailsViewModel()
                {
                    Id = p.Id,
                    ProjectName=p.ProjectName,
                    Description = p.Description,
                })
                .OrderBy(p => p.ProjectName)
                .ToList();

            return View(models);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProjectsCreateViewModel model)
        {
            if (!ModelState.IsValid) 
            { 
              return View(model);
            }
            Project p=new Project() 
            {
                ProjectName=model.Projectname,
                Description = model.Description 
            }; 
            context.Projects.Add(p);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            ProjectDetailsViewModel? project = context
                .Projects
                .AsNoTracking()
                .Include(p => p.Teams)
                .Select(p => new ProjectDetailsViewModel
                {
                    Id = p.Id,
                    ProjectName = p.ProjectName,
                    Description = p.Description,
                    Teams = p.Teams.ToList()
                })
                .FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ProjectEditViewModel? model = context
                .Projects
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProjectEditViewModel
                {
                    Id = p.Id,
                    ProjectName = p.ProjectName,
                    Description = p.Description
                })
                .FirstOrDefault();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(ProjectEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Project? project = context.Projects.Find(model.Id);

            if (project == null)
            {
                return NotFound();
            }

            project.ProjectName = model.ProjectName;
            project.Description = model.Description;

            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        //Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            ProjectDeleteViewModel? model = context
                .Projects
                .AsNoTracking()
                .Include(p => p.Teams)
                .Where(p => p.Id == id)
                .Select(p => new ProjectDeleteViewModel
                {
                    Id = p.Id,
                    ProjectName = p.ProjectName,
                    Description = p.Description,
                    Teams = p.Teams
                        .Select(t => new TeamSimpleViewModel
                        {
                            Id = t.Id,
                            TeamName = t.TeamName
                        })
                        .ToList()
                })
                .FirstOrDefault();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            Project? project = context.Projects.Find(id);

            if (project == null)
            {
                return NotFound();
            }

            context.Projects.Remove(project);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
