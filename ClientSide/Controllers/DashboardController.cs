using ClientSide.Contract;
using ClientSide.Utilities.Enum;
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
            var components = new ComponentHandlers
            {
                Footer = false,
                SideBar = true,
                Navbar = true,
            };
            ViewBag.Components = components;
            if (User.IsInRole(nameof(RoleLevel.ProjectManager)))
            {
                var assignment = await _assignmentRepository.GetFromManager(Guid.Parse(HttpContext.User.FindFirstValue("Guid")));
                var month = await _dashboardRepository.CountMonth(Guid.Parse(HttpContext.User.FindFirstValue("Guid")));
                var category = await _dashboardRepository.CountCategory(Guid.Parse(HttpContext.User.FindFirstValue("Guid")));

                DashboardHandlersManager data;
                if (assignment.Code == 404)
                {
                    data = new DashboardHandlersManager
                    {
                        totalAssignment = 0
                    };
                }
                else
                {
                    data = new DashboardHandlersManager
                    {
                        totalAssignment = assignment.Data.Count(),
                        Count = month.Data.Count,
                        Mount = month.Data.Month,
                        Category = category.Data.CategoryName,
                        CountCategory = category.Data.Count
                    };
                }
                return View("ProjectManager", data);
            }
            else if (User.IsInRole(nameof(RoleLevel.Admin)))
            {
                var role = await _dashboardRepository.CountRole();


                return View("Admin", role.Data);
            }
            else
            {
                return View("Staff");
            }

        }
    }
}