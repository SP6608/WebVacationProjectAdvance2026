using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebVacantionManager.Data;
using WebVacantionManager.Models;
using WebVacantionManager.ViewModels;

namespace WebVacantionManager.Controllers
{
    [Authorize(Roles = "Ceo")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public UsersController(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await context.Users
                .AsNoTracking()
                .Include(u => u.Team)
                .ToListAsync();

            List<UsersIndexViewModel> model = new List<UsersIndexViewModel>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);

                model.Add(new UsersIndexViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RoleName = roles.FirstOrDefault(),
                    TeamName = user.Team != null ? user.Team.TeamName : null
                });
            }

            return View(model);
        }
        public IActionResult Details(string id)
        {
            AppUser? user = context.Users
                .AsNoTracking()
                .Include(u => u.Team)
                .Include(u => u.VacationRequests)
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            string? roleName = userManager.GetRolesAsync(user).Result.FirstOrDefault();

            UsersDetailsViewModel model = new UsersDetailsViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                RoleName = roleName,
                TeamName = user.Team != null ? user.Team.TeamName : null,
                VacationRequestsCount = user.VacationRequests.Count
            };

            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            AppUser? user = context.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            string? currentRole = userManager.GetRolesAsync(user).Result.FirstOrDefault();

            UsersEditViewModel model = new UsersEditViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TeamId = user.TeamId,
                RoleName = currentRole,
                Teams = context.Teams
                    .AsNoTracking()
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.TeamName
                    })
                    .ToList(),
                Roles = roleManager.Roles
                    .AsNoTracking()
                    .Select(r => new SelectListItem
                    {
                        Value = r.Name!,
                        Text = r.Name!
                    })
                    .ToList()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UsersEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Teams = context.Teams
                    .AsNoTracking()
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.TeamName
                    })
                    .ToList();

                model.Roles = roleManager.Roles
                    .AsNoTracking()
                    .Select(r => new SelectListItem
                    {
                        Value = r.Name!,
                        Text = r.Name!
                    })
                    .ToList();

                return View(model);
            }

            AppUser? user = context.Users.FirstOrDefault(u => u.Id == model.Id);

            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.TeamId = model.TeamId;

            var currentRoles = userManager.GetRolesAsync(user).Result;

            if (currentRoles.Any())
            {
                var removeResult = userManager.RemoveFromRolesAsync(user, currentRoles).Result;

                if (!removeResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Unable to remove current role.");
                }
            }

            if (!string.IsNullOrWhiteSpace(model.RoleName))
            {
                var addResult = userManager.AddToRoleAsync(user, model.RoleName).Result;

                if (!addResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Unable to assign new role.");
                }
            }

            if (!ModelState.IsValid)
            {
                model.Teams = context.Teams
                    .AsNoTracking()
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.TeamName
                    })
                    .ToList();

                model.Roles = roleManager.Roles
                    .AsNoTracking()
                    .Select(r => new SelectListItem
                    {
                        Value = r.Name!,
                        Text = r.Name!
                    })
                    .ToList();

                return View(model);
            }

            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}