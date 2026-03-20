using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebVacantionManager.Models;

namespace WebVacantionManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Home/Error/{statusCode?}")]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode == 404)
            {
                return View("Error404");
            }

            return View("Error500");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
