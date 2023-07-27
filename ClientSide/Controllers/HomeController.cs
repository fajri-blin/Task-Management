﻿using ClientSide.Contract;
using ClientSide.Models;
using ClientSide.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ClientSide.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAssignmentRepository _assignmentRepository;

        public HomeController(IAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        public async Task<IActionResult> Index()
        {
            var assignment = await _assignmentRepository.GetFromManager(Guid.Parse(HttpContext.User.FindFirstValue("Guid")));
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
                    totalAssignment = assignment.Data.Count()
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