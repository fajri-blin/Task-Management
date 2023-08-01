using ClientSide.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ClientSide.Contract;
using Task_Management.Utilities.Enum;
using ClientSide.ViewModels.Progress;
using Microsoft.EntityFrameworkCore;


namespace ClientSide.Controllers;

[Controller]
public class ProgressController : Controller
{
    private readonly IProgressRepository _progressRepository;

    public ProgressController(IProgressRepository progressRepository)
    {
        _progressRepository = progressRepository;
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
        var tasks = response.Data.ToList();
        return View(tasks);
    }

    [HttpPost]
    [Route("Progress/UpdateStatus")]
    public async Task<IActionResult> UpdateStatus(Guid guid, string newStatus)
    {
        var response = await _progressRepository.GetProgressById(guid);
        var progress = response.Data;

        if (progress != null)
        {
            progress.Status = Enum.Parse<StatusEnum>(newStatus);
            return Json(new { success = true });
        }

        return Json(new { success = false });
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
}
