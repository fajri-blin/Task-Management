using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClientSide.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IAssignmentRepository assignmentRepository, IDashboardRepository boardRepository)
        {
            _assignmentRepository = assignmentRepository;
            _dashboardRepository = boardRepository;
        }

        public async Task<IActionResult> Index()
        {
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
    }
}