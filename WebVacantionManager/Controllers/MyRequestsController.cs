using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebVacantionManager.Data;
using WebVacantionManager.ViewModels;

namespace WebVacantionManager.Controllers
{
    public class MyRequestsController : Controller
    {
        private readonly ApplicationDbContext context;
        public MyRequestsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var model = context.VacationRequests
                .AsNoTracking()
                .Include(v => v.Applicant)
                .Select(v => new VacationRequestIndexViewModel
                {
                    Id = v.Id,
                    ApplicantName = v.Applicant.FirstName + " " + v.Applicant.LastName,
                    DateFrom = v.DateFrom,
                    DateTo = v.DateTo,
                    CreatedOn = v.CreatedOn,
                    VacationType = v.VacationType.ToString(),
                    IsHalfDay = v.IsHalfDay,
                    IsApproved = v.IsApproved
                })
                .ToList();

            return View(model);
        }
    }
}
