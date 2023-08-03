using ClientSide.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ClientSide.Contract;
using Task_Management.Utilities.Enum;
using ClientSide.ViewModels.Progress;
using Microsoft.EntityFrameworkCore;
using ClientSide.ViewModels.Assignment;
using System;
using ClientSide.ViewModels.Profile;
using Syncfusion.EJ2.Grids;

namespace ClientSide.Controllers;

[Controller]
public class ProgressController : Controller
{
    private readonly IProgressRepository _progressRepository;
    private readonly IAssignmentRepository _assignmentRepository;

    public ProgressController(IProgressRepository progressRepository, IAssignmentRepository assignmentRepository)
    {
        _progressRepository = progressRepository;
        _assignmentRepository = assignmentRepository;
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

        var response = await _progressRepository.GetAllProgress();
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
        return View( "CreateProgress", createProgressVM);
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
                return RedirectToAction(nameof(Index));
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
            DueDate = result.Data.DueDate,
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

}
