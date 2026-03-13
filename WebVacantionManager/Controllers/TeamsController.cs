using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebVacantionManager.Data;
using WebVacantionManager.Models;
using WebVacantionManager.ViewModels;

namespace WebVacantionManager.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext context;
        public TeamsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            List<TeamIndexViewModel> model = this.context
                .Teams
                .AsNoTracking()
                .Include(t => t.Project)
                .Include(t => t.TeamLeader)
                .Include(t => t.Developers)
                .Select(t => new TeamIndexViewModel
                {
                    Id = t.Id,
                    TeamName = t.TeamName,
                    ProjectName = t.Project != null ? t.Project.ProjectName : null,
                    TeamLeaderName = t.TeamLeader != null ? t.TeamLeader.UserName : null,
                    DevelopersCount = t.Developers.Count
                }).ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            TeamCreateViewModel model = new TeamCreateViewModel
            {
                Projects = context.Projects
                    .AsNoTracking()
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.ProjectName
                    })
                    .ToList(),

                TeamLeaders = context.Users
                    .AsNoTracking()
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = u.UserName!
                    })
                    .ToList()
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Create(TeamCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Projects = context.Projects
                    .AsNoTracking()
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.ProjectName
                    })
                    .ToList();

                model.TeamLeaders = context.Users
                    .AsNoTracking()
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = u.UserName!
                    })
                    .ToList();

                return View(model);
            }

            Team team = new Team
            {
                TeamName = model.TeamName,
                ProjectId = model.ProjectId,
                TeamLeaderId = model.TeamLeaderId
            };

            context.Teams.Add(team);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            Team? team = context.Teams
                .AsNoTracking()
                .Include(t => t.Project)
                .Include(t => t.TeamLeader)
                .Include(t => t.Developers)
                .FirstOrDefault(t => t.Id == id);
            if (team == null)
            {
                return NotFound();
            }
            TeamsDetailsViewModel model = new TeamsDetailsViewModel
            {
                Id = team.Id,
                TeamName = team.TeamName,
                ProjectName = team.Project != null ? team.Project.ProjectName : null,
                TeamLeaderName = team.TeamLeader != null ? team.TeamLeader.UserName : null,
                Developers = team.Developers.ToList()
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Team? team = context.Teams
                .AsNoTracking()
                .Include(t => t.Project)
                .Include(t => t.TeamLeader)
                .FirstOrDefault(t => t.Id == id);

            if (team == null)
            {
                return NotFound();
            }

            TeamsEditViewModel model = new TeamsEditViewModel
            {
                Id = team.Id,
                TeamName = team.TeamName,
                ProjectId = team.ProjectId,
                TeamLeaderId = team.TeamLeaderId,
                Projects = context.Projects
                    .AsNoTracking()
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.ProjectName
                    })
                    .ToList(),
                TeamLeaders = context.Users
                    .AsNoTracking()
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = u.UserName!
                    })
                    .ToList()
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(TeamsEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Projects = context.Projects
                    .AsNoTracking()
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.ProjectName
                    })
                    .ToList();

                model.TeamLeaders = context.Users
                    .AsNoTracking()
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = u.UserName!
                    })
                    .ToList();

                return View(model);
            }

            Team? team = context.Teams.Find(model.Id);
            if (team == null)
            {
                return NotFound();
            }

            team.TeamName = model.TeamName;
            team.ProjectId = model.ProjectId;
            team.TeamLeaderId = model.TeamLeaderId;

            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "CEO")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Team? team = context.Teams
                .AsNoTracking()
                .Include(t => t.Project)
                .Include(t => t.TeamLeader)
                .Include(t => t.Developers)
                .FirstOrDefault(t => t.Id == id);

            if (team == null)
            {
                return NotFound();
            }

            TeamsDeleteViewModel model = new TeamsDeleteViewModel
            {
                Id = team.Id,
                TeamName = team.TeamName,
                ProjectName = team.Project != null ? team.Project.ProjectName : null,
                TeamLeaderName = team.TeamLeader != null
                    ? $"{team.TeamLeader.FirstName} {team.TeamLeader.LastName}"
                    : null,
                Developers = team.Developers.ToList()
            };

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "CEO")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Team? team = context.Teams.Find(id);

            if (team == null)
            {
                return NotFound();
            }

            context.Teams.Remove(team);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
    

