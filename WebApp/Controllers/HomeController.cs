using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
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

            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Search", "Service");

            return RedirectToAction("Login", "Auth");
        }
    }
}
