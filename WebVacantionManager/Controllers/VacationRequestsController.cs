using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebVacantionManager.Data;
using WebVacantionManager.Models;
using WebVacantionManager.ViewModels;
using WebVacationManager.Models;

namespace WebVacantionManager.Controllers
{
    [Authorize]
    public class VacationRequestsController : Controller
    {
        private readonly ApplicationDbContext context;

        public VacationRequestsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            AppUser? currentUser = context.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == currentUserId);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            IQueryable<VacationRequest> query = context.VacationRequests
                .AsNoTracking()
                .Include(v => v.Applicant);

            if (User.IsInRole("Ceo"))
            {
                // Ceo вижда всички заявки
            }
            else if (User.IsInRole("TeamLead"))
            {
                query = query.Where(v => v.Applicant.TeamId == currentUser.TeamId);
            }
            else
            {
                query = query.Where(v => v.ApplicantId == currentUserId);
            }

            var model = query
                .Select(v => new VacationRequestIndexViewModel
                {
                    Id = v.Id,
                    ApplicantName = v.Applicant.FirstName + " " + v.Applicant.LastName,
                    DateFrom = v.DateFrom,
                    DateTo = v.DateTo,
                    VacationType = v.VacationType.ToString(),
                    IsApproved = v.IsApproved
                })
                .ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new VacationRequestCreateViewModel
            {
                VacationTypes = GetVacationTypes()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VacationRequestCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.VacationTypes = GetVacationTypes();
                return View(model);
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            bool isHalfDay = model.VacationType == VacationType.SickLeave
                ? false
                : model.IsHalfDay;

            VacationRequest request = new VacationRequest
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo,
                VacationType = model.VacationType,
                ApplicantId = userId,
                CreatedOn = DateTime.UtcNow,
                IsApproved = false,
                IsHalfDay = isHalfDay
            };

            context.VacationRequests.Add(request);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Ceo,TeamLead")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id)
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            AppUser? currentUser = context.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == currentUserId);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            VacationRequest? request = context.VacationRequests
                .Include(v => v.Applicant)
                .FirstOrDefault(v => v.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            if (request.IsApproved)
            {
                return RedirectToAction(nameof(Index));
            }

            bool canApprove = false;

            if (User.IsInRole("Ceo"))
            {
                canApprove = true;
            }
            else if (User.IsInRole("TeamLead"))
            {
                if (currentUser.TeamId != null &&
                    request.Applicant.TeamId == currentUser.TeamId &&
                    request.ApplicantId != currentUserId)
                {
                    canApprove = true;
                }
            }

            if (!canApprove)
            {
                return Forbid();
            }

            request.IsApproved = true;
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            AppUser? currentUser = context.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == currentUserId);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            VacationRequest? request = context.VacationRequests
                .AsNoTracking()
                .Include(v => v.Applicant)
                .FirstOrDefault(v => v.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            bool canView = false;

            if (User.IsInRole("Ceo"))
            {
                canView = true;
            }
            else if (User.IsInRole("TeamLead"))
            {
                canView = request.Applicant.TeamId == currentUser.TeamId;
            }
            else
            {
                canView = request.ApplicantId == currentUserId;
            }

            if (!canView)
            {
                return Forbid();
            }

            VacationRequestDetailsViewModel model = new VacationRequestDetailsViewModel
            {
                Id = request.Id,
                ApplicantName = request.Applicant.FirstName + " " + request.Applicant.LastName,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo,
                CreatedOn = request.CreatedOn,
                VacationType = request.VacationType.ToString(),
                IsHalfDay = request.IsHalfDay,
                IsApproved = request.IsApproved
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            VacationRequest? request = context.VacationRequests
                .AsNoTracking()
                .FirstOrDefault(v => v.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            if (request.ApplicantId != currentUserId || request.IsApproved)
            {
                return Forbid();
            }

            VacationRequestEditViewModel model = new VacationRequestEditViewModel
            {
                Id = request.Id,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo,
                IsHalfDay = request.IsHalfDay,
                VacationType = request.VacationType,
                VacationTypes = GetVacationTypes()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(VacationRequestEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.VacationTypes = GetVacationTypes();
                return View(model);
            }

            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            VacationRequest? request = context.VacationRequests
                .FirstOrDefault(v => v.Id == model.Id);

            if (request == null)
            {
                return NotFound();
            }

            if (request.ApplicantId != currentUserId || request.IsApproved)
            {
                return Forbid();
            }

            request.DateFrom = model.DateFrom;
            request.DateTo = model.DateTo;
            request.VacationType = model.VacationType;
            request.IsHalfDay = model.VacationType == VacationType.SickLeave
                ? false
                : model.IsHalfDay;

            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            VacationRequest? request = context.VacationRequests
                .AsNoTracking()
                .Include(v => v.Applicant)
                .FirstOrDefault(v => v.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            if (request.ApplicantId != currentUserId || request.IsApproved)
            {
                return Forbid();
            }

            VacationRequestDeleteViewModel model = new VacationRequestDeleteViewModel
            {
                Id = request.Id,
                ApplicantName = request.Applicant.FirstName + " " + request.Applicant.LastName,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo,
                CreatedOn = request.CreatedOn,
                VacationType = request.VacationType.ToString(),
                IsHalfDay = request.IsHalfDay,
                IsApproved = request.IsApproved
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            VacationRequest? request = context.VacationRequests
                .FirstOrDefault(v => v.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            if (request.ApplicantId != currentUserId || request.IsApproved)
            {
                return Forbid();
            }

            context.VacationRequests.Remove(request);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private IEnumerable<SelectListItem> GetVacationTypes()
        {
            return Enum.GetValues(typeof(VacationType))
                .Cast<VacationType>()
                .Select(v => new SelectListItem
                {
                    Value = v.ToString(),
                    Text = v.ToString()
                })
                .ToList();
        }
    }
}