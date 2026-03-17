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

        public UsersController(
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
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

        public async Task<IActionResult> Details(string id)
        {
            AppUser? user = await context.Users
                .AsNoTracking()
                .Include(u => u.Team)
                .Include(u => u.VacationRequests)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            string? roleName = (await userManager.GetRolesAsync(user)).FirstOrDefault();

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
        public async Task<IActionResult> Edit(string id)
        {
            AppUser? user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            string? currentRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();

            UsersEditViewModel model = new UsersEditViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TeamId = user.TeamId,
                RoleName = currentRole,
                Teams = await context.Teams
                    .AsNoTracking()
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.TeamName
                    })
                    .ToListAsync(),
                Roles = await roleManager.Roles
                    .AsNoTracking()
                    .Select(r => new SelectListItem
                    {
                        Value = r.Name!,
                        Text = r.Name!
                    })
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsersEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Teams = await context.Teams
                    .AsNoTracking()
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.TeamName
                    })
                    .ToListAsync();

                model.Roles = await roleManager.Roles
                    .AsNoTracking()
                    .Select(r => new SelectListItem
                    {
                        Value = r.Name!,
                        Text = r.Name!
                    })
                    .ToListAsync();

                return View(model);
            }

            AppUser? user = await context.Users.FirstOrDefaultAsync(u => u.Id == model.Id);

            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.TeamId = model.TeamId;

            var currentRoles = await userManager.GetRolesAsync(user);

            if (currentRoles.Any())
            {
                var removeResult = await userManager.RemoveFromRolesAsync(user, currentRoles);

                if (!removeResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Unable to remove current role.");
                }
            }

            if (!string.IsNullOrWhiteSpace(model.RoleName))
            {
                var addResult = await userManager.AddToRoleAsync(user, model.RoleName);

                if (!addResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Unable to assign new role.");
                }
            }

            if (!ModelState.IsValid)
            {
                model.Teams = await context.Teams
                    .AsNoTracking()
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.TeamName
                    })
                    .ToListAsync();

                model.Roles = await roleManager.Roles
                    .AsNoTracking()
                    .Select(r => new SelectListItem
                    {
                        Value = r.Name!,
                        Text = r.Name!
                    })
                    .ToListAsync();

                return View(model);
            }

            await context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Потребителят е редактиран успешно.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser? user = await context.Users
                .AsNoTracking()
                .Include(u => u.Team)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            string? roleName = (await userManager.GetRolesAsync(user)).FirstOrDefault();

            UsersDeleteViewModel model = new UsersDeleteViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                RoleName = roleName,
                TeamName = user.Team != null ? user.Team.TeamName : null
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            AppUser? user = await context.Users
                .Include(u => u.VacationRequests)
                .Include(u => u.Team)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            bool hasVacationRequests = user.VacationRequests.Any();

            bool isTeamLeader = await context.Teams
                .AnyAsync(t => t.TeamLeaderId == user.Id);

            bool isDeveloperInTeam = await context.Teams
                .AnyAsync(t => t.Developers.Any(d => d.Id == user.Id));

            if (hasVacationRequests || isTeamLeader || isDeveloperInTeam)
            {
                TempData["ErrorMessage"] = "Потребителят не може да бъде изтрит, защото има свързани записи.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            var roles = await userManager.GetRolesAsync(user);

            if (roles.Any())
            {
                var removeRolesResult = await userManager.RemoveFromRolesAsync(user, roles);

                if (!removeRolesResult.Succeeded)
                {
                    TempData["ErrorMessage"] = "Неуспешно премахване на ролите на потребителя.";
                    return RedirectToAction(nameof(Delete), new { id });
                }
            }

            var deleteResult = await userManager.DeleteAsync(user);

            if (!deleteResult.Succeeded)
            {
                TempData["ErrorMessage"] = "Неуспешно изтриване на потребителя.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            TempData["SuccessMessage"] = "Потребителят е изтрит успешно.";
            return RedirectToAction(nameof(Index));
        }
    }
}