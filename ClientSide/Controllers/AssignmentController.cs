using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Assignment;
using Microsoft.AspNetCore.Mvc;

namespace ClientSide.Controllers;

[Controller]
public class AssignmentController : Controller
{   
    private readonly IAssignmentRepository _assignmentRepository;

    public AssignmentController(IAssignmentRepository assignmentRepository)
    {
        _assignmentRepository = assignmentRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAssignment()
    {
        var result = await _assignmentRepository.Get();
        var listAssignment = new List<AssignmentVM>();

        if (result != null)
        {
            listAssignment = result.Data.Select(entity => new AssignmentVM
            {
                Guid = entity.Guid,
                ManagerGuid = entity.ManagerGuid,
                Title = entity.Title,
                Description = entity.Description,
                DueDate = entity.DueDate,
                Category = entity.Category,
                Progress = entity.Progress,
            }).ToList();
        }

        var components = new ComponentHandlers
        {
            Footer = false,
            SideBar = true,
            Navbar = true,
        };
        ViewBag.Components = components;

        return View("GetAllAssignment",listAssignment);
    }
}
