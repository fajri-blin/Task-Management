using ClientSide.Models;
using ClientSide.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClientSide.Controllers
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
            var components = new ComponentHandlers
            {
                Footer = false,
                SideBar = true,
                Navbar = true,
            };
            ViewBag.Components = components;

            var jwt = HttpContext.Session.GetString("JWToken");

            var data = new 
            {
                token = jwt,
            };

            return View(data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}