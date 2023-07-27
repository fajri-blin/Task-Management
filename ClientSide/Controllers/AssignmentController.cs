using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Assignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClientSide.Controllers;

[Controller]
public class AssignmentController : Controller
{   
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IAccountRepository _accountRepository;

    public AssignmentController(IAssignmentRepository assignmentRepository)
    {
        _assignmentRepository = assignmentRepository;
    }

    [HttpPost]
    public async Task<IActionResult> AddAssignment(CreateAssignmentVM assignment)
    {
        var created = await _assignmentRepository.AddAssignment(assignment);
        if (created.Code == 404) 
        {
            ModelState.AddModelError(string.Empty, created.Message);
            return View();
        }
        TempData["Success"] = "Data Berhasil Masuk";
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public IActionResult AddAssignment()
    {
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> DeepDeleteAssignments(Guid guid)
    {
        var components = new ComponentHandlers
        {
            Footer = false,
            SideBar = true,
            Navbar = true,
        };
        ViewBag.Components = components;

        var result = await _assignmentRepository.DeepDeleteAssignments(guid);

        if (result.Code == 404)
        {
            return new JsonResult(new { code = 404, message = "Not found" });
        }
        return new JsonResult(new { code = 200, message = "Data was found", status = "OK", data = (string)null });
    }



    [HttpGet]
    [Authorize] // Make sure the action requires authentication
    public async Task<IActionResult> GetAllAssignment()
    {
        var components = new ComponentHandlers
        {
            Footer = false,
            SideBar = true,
            Navbar = true,
        };
        ViewBag.Components = components;

        // Access the Guid claim from the User principal
        var userIdClaim = User.FindFirstValue("Guid");

        // Check if the Guid claim exists and is valid
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            // Handle the case where the Guid claim is missing or invalid
            return Unauthorized(); // Or any other response you prefer
        }

        var result = await _assignmentRepository.GetFromManager(userId);
        var listAssignment = new List<AssignmentVM>();

        if (result.Code == 404)
        {
            return View("../Error/NotFound");
        }
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

        return View("GetAllAssignment", listAssignment);
    }
}