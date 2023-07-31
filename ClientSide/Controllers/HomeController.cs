using ClientSide.Contract;
using ClientSide.Models;
using ClientSide.Utilities.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ClientSide.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IDashboardRepository _dashboardRepository;

        public HomeController(IAssignmentRepository assignmentRepository, IDashboardRepository boardRepository)
        {
            _assignmentRepository = assignmentRepository;
            _dashboardRepository = boardRepository;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Account");
            }

            var assignment = await _assignmentRepository.GetFromManager(Guid.Parse(HttpContext.User.FindFirstValue("Guid")));
            var month = await _dashboardRepository.CountMonth(Guid.Parse(HttpContext.User.FindFirstValue("Guid")));
            var category = await _dashboardRepository.CountCategory(Guid.Parse(HttpContext.User.FindFirstValue("Guid")));
            var components = new ComponentHandlers
            {
                Footer = false,
                SideBar = true,
                Navbar = true,
            };
            ViewBag.Components = components;
            DashboardHandlers data;
            if (assignment.Code == 404)
            {
                data = new DashboardHandlers
                {
                    totalAssignment = 0
                };
            }
            else
            {
                data = new DashboardHandlers
                {
                    totalAssignment = assignment.Data.Count(),
                    Count = month.Data.Count,
                    Mount = month.Data.Month,
                    Category = category.Data.CategoryName,
                    CountCategory = category.Data.Count
                };
            }
            return View("Index", data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}