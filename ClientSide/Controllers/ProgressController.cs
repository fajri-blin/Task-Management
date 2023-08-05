using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Progress;
using Microsoft.EntityFrameworkCore;
using ClientSide.ViewModels.Assignment;
using System;
using ClientSide.ViewModels.Profile;
using Syncfusion.EJ2.Grids;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using ClientSide.Utilities.Enum;
using ClientSide.ViewModels.AccountProgress;

namespace ClientSide.Controllers;

[Controller]
public class ProgressController : Controller
{
    private readonly IProgressRepository _progressRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountProgressRepository _accountProgressRepository;

    public ProgressController(IProgressRepository progressRepository, 
                              IAssignmentRepository assignmentRepository,
                              IAccountRepository accountRepository,
                              IAccountProgressRepository accountProgressRepository)
    {
        _progressRepository = progressRepository;
        _assignmentRepository = assignmentRepository;
        _accountRepository = accountRepository;
        _accountProgressRepository = accountProgressRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index(Guid guid)
    {
        var components = new ComponentHandlers
        {
            Footer = false,
            SideBar = true,
            Navbar = true,
        };
        ViewBag.Components = components;

        var response = await _progressRepository.GetAllProgress(guid);
        if (response.Data != null)
        {
            var tasks = response.Data.ToList();
            return View(tasks);
        }
        else
        {
            var emptyList = new List<ProgressVM>();
            return View(emptyList);
        }
    }

    [HttpGet]
    public IActionResult CreateProgress(Guid assignmentGuid)
    {
        var components = new ComponentHandlers
        {
            Footer = false,
            SideBar = true,
            Navbar = true,
        };
        ViewBag.Components = components;
        var createProgressVM = new CreateProgressVM
        {
            AssignmentGuid = assignmentGuid
        };
        return View("CreateProgress", createProgressVM);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProgress(CreateProgressVM createProgress)
    {
        if (ModelState.IsValid)
        {
            var createdProgress = await _progressRepository.CreateProgress(createProgress);
            if (createdProgress != null)
            {
                TempData["Success"] = "Data Berhasil Masuk";
                return RedirectToAction(nameof(Index), new { guid = createProgress.AssignmentGuid });
            }
            ModelState.AddModelError(string.Empty, "Failed to create progress.");
        }
        return View(createProgress);
    }




    [HttpPost]
    public async Task<IActionResult> DeepDeleteProgress(Guid guid)
    {
        var result = await _progressRepository.DeepDeleteProgress(guid);

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
    public async Task<IActionResult> EditProgress(Guid guid)
    {
        var components = new ComponentHandlers
        {
            Footer = false,
            SideBar = true,
            Navbar = true,
        };
        ViewBag.Components = components;

        var result = await _progressRepository.Get(guid);
        if (result.Data == null)
        {
            return NotFound();
        }

        var updateProgressVM = new UpdateProgressVM
        {
            Guid = result.Data.Guid,
            AssignmentGuid = result.Data.AssignmentGuid,
            Description = result.Data.Description,
            Status = result.Data.Status,
            Additional = result.Data.Additional,
            MessageManager = result.Data.MessageManager,
            /*DueDate = result.Data.DueDate,*/
        };

        return View(updateProgressVM);
    }
    [HttpPost]
    public async Task<IActionResult> EditProgress(UpdateProgressVM updateProgress)
    {

        var result = await _progressRepository.UpdateProgress(updateProgress);
        if (result.Code == 200)
        {
            TempData["Success"] = "Data Berhasil Diupdate";
            return RedirectToAction(nameof(Index));
        }
        else if (result.Code == 400)
        {
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

        return View("Index");
    }

    [HttpGet]
    public async Task<IActionResult> AddStaff(Guid guid, Guid assignmentGuid)
    {
        var components = new ComponentHandlers
        {
            Footer = false,
            SideBar = true,
            Navbar = true,
        };
        ViewBag.Components = components;

        var response = await _accountRepository.Get();
        var staffList = response.Data
            .Where(staff => staff.Role == RoleLevel.Staff) // Filter staff with RoleLevel.Staff
            .Select(staff => new AddStaffVM
            {
                Guid = staff.Guid,
                Name = staff.Name,
                Role = staff.Role,
            })
            .ToList();

        ViewBag.ProgressGuid = guid;
        ViewBag.AssignmentGuid = assignmentGuid; // Set the ViewBag.AssignmentGuid here
        return View("AddStaff", staffList);
    }
    [HttpPost]
    public async Task<IActionResult> AssignStaff(Guid progressGuid, Guid assignmentGuid, List<Guid> selectedStaffGuids)
    {
        try
        {
            var getAccountProgress = await _accountProgressRepository.GetByProgress(progressGuid);
            if (getAccountProgress.Data != null)
            {
                foreach (var item in getAccountProgress.Data)
                {
                    await _accountProgressRepository.DeleteAccountProgress(item.Guid);
                }
            }

            foreach (var item in selectedStaffGuids)
            {
                var dto = new AssignStaff
                {
                    AccountGuid = item,
                    ProgressGuid = progressGuid,
                };
                await _accountProgressRepository.AddAccountProgress(dto);
            }

            // Redirect to the appropriate action after the staff is assigned.
            return RedirectToAction("Index", new { guid = assignmentGuid });
        }
        catch (Exception ex)
        {
            // Handle the error appropriately (e.g., log the error, show an error message, etc.).
            return View("Error");
        }
    }

}
