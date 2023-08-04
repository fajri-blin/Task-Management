using ClientSide.Contract;
using ClientSide.Utilities.Enum;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Assignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClientSide.Controllers;

[Controller]
public class AssignmentController : Controller
{   
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;

    public AssignmentController(IAssignmentRepository assignmentRepository, ICategoryRepository categoryRepository)
    {
        _assignmentRepository = assignmentRepository;
        _categoryRepository = categoryRepository;
    }

    private string GetManagerGuidFromToken()
    {
        // Access the JWT token from the User.Claims
        var userIdClaim = User.FindFirstValue("Guid");

        // Check if the ManagerGuid claim exists and is valid
        if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var managerGuid))
        {
            // Return the ManagerGuid as a string
            return managerGuid.ToString();
        }

        // Return null if ManagerGuid is not found
        return null;
    }

    [Authorize(Roles = nameof(RoleLevel.Staff))]
    [HttpGet]
    public async Task<IActionResult> GetProgressForStaff()
    {
        var accountIdClaim = User.FindFirstValue("Guid");
        if (!Guid.TryParse(accountIdClaim, out var accountId))
        {
            // Handle the case where the accountId is missing or invalid
            return BadRequest("Invalid accountId");
        }

        var components = new ComponentHandlers
        {
            Footer = false,
            SideBar = true,
            Navbar = true,
        };
        ViewBag.Components = components;

        var response = await _assignmentRepository.GetProgressForStaff(accountId);
        if (response.Code != 200)
        {
            return NotFound();
        }

        // Assuming the data property in the response is of type IEnumerable<GetForStaffVM>
        var getForStaffVMList = response.Data;

        return View(getForStaffVMList);
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
        return RedirectToAction(nameof(GetAllAssignment));
    }
    [HttpGet]
    public IActionResult AddAssignment()
    {
        var components = new ComponentHandlers
        {
            Footer = false,
            SideBar = true,
            Navbar = true,
        };
        ViewBag.Components = components;

        // Call the method to set ManagerGuid in the session
        var managerGuid = GetManagerGuidFromToken();

        // Pass the ManagerGuid to the view
        ViewBag.ManagerGuid = managerGuid;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> DeepDeleteAssignments(Guid guid)
    {
        var result = await _assignmentRepository.DeepDeleteAssignment(guid);

        if (result.Code == 404)
        {
            return Json(new { code = 404, message = "Assignment not found" });
        }
        else if (result.Code == 200)
        {
            return Json(new { code = 200, message = "Assignment deleted successfully" });
        }
        else
        {
            return Json(new { code = result.Code, message = result.Message });
        }
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
                Title = entity.Title,
                Description = entity.Description,
                DueDate = entity.DueDate,
                Category = entity.Category,
                Progress = entity.Progress,
            }).ToList();
        }

        return View("GetAllAssignment", listAssignment);
    }


    [HttpGet]
    public async Task<IActionResult> EditAssignment(Guid guid)
    {
        var components = new ComponentHandlers
        {
            Footer = false,
            SideBar = true,
            Navbar = true,
        };
        ViewBag.Components = components;

        // Retrieve the assignment data
        var result = await _assignmentRepository.Get(guid);
        if (result.Data == null)
        {
            return NotFound();
        }

        // Map AssignmentVM to UpdateAssignmentVM (assuming both have similar properties)
        var updateAssignmentVM = new UpdateAssignmentVM
        {
            Guid = result.Data.Guid,
            Title = result.Data.Title,
            Description = result.Data.Description,
            DueDate = result.Data.DueDate,
            Category = result.Data.Category
        };

        // Create a SelectList for the selected categories
        var selectedCategories = updateAssignmentVM.Category?.Select(category => new SelectListItem
        {
            Value = category,
            Text = category
        }).ToList();

        return View(updateAssignmentVM);
    }

    [HttpPost]
    public async Task<IActionResult> EditAssignment(UpdateAssignmentVM updateAssignmentVM)
    {
        // Map AssignmentVM to UpdateAssignmentVM (assuming both have similar properties)
        var result = await _assignmentRepository.Update(updateAssignmentVM);
        if (result.Code == 200)
        {
            TempData["Success"] = "Data Berhasil Diupdate";
            return RedirectToAction(nameof(GetAllAssignment));
        }
        else if (result.Code == 400)
        {
            // If there is a validation error related to Categories,
            // populate the ModelState with the error message
            if (result.Data != null && result.Data is IDictionary<string, string[]> validationErrors)
            {
                foreach (var error in validationErrors)
                {
                    foreach (var errorMessage in error.Value)
                    {
                        ModelState.AddModelError(error.Key, errorMessage);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
            }
        }
        else if (result.Code == 409)
        {
            ModelState.AddModelError(string.Empty, result.Message);
        }

        return View("GetAllAssignment");
    }

}