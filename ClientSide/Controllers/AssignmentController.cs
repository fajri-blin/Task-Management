using ClientSide.Contract;
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
        var listUniversity = new List<AssignmentVM>();

        if (result != null)
        {
            listUniversity = result.Data.Select(entity => new AssignmentVM
            {
                Guid = entity.Guid,
                Title = entity.Title,
                Description = entity.Description,
                DueDate = entity.DueDate,
                IsCompleted = entity.IsCompleted,
                ManagerGuid = entity.ManagerGuid,
            }).ToList();
        }

        return View(listUniversity);
    }
}
