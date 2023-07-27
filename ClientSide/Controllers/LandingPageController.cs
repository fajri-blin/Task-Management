using ClientSide.Contract;
using Microsoft.AspNetCore.Mvc;

namespace ClientSide.Controllers
{
    [Controller]
    public class LandingPageController : Controller
    {
        private readonly ILogger<LandingPageController> _logger;

        public LandingPageController(ILogger<LandingPageController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Landing page accessed.");
            return View();
        }
    }
}
