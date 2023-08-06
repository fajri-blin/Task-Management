using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Task_Management.Dtos.AssignmentDto;
using Task_Management.DTOs.AssignmentDto;
using Task_Management.Service;
using Task_Management.Utilities.Enum;
using Task_Management.Utilities.Handler;

namespace Task_Management.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssignmentController : ControllerBase
{
    private readonly AssignmentService _assignmentServices;

    public AssignmentController(AssignmentService accountSevices)
    {
        _assignmentServices = accountSevices;
    }

    [HttpGet("GetForStaff/{guid}")]
    [Authorize]
    public IActionResult GetForStaff(Guid guid)
    {
        var entity = _assignmentServices.GetForStaff(guid);
        if (entity == null) return NotFound(new ResponseHandlers<GetForStaffDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<IEnumerable<GetForStaffDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entity
        });
    }

    [HttpDelete("DeepDelete/{guid}")]
    [Authorize(Roles = $"{nameof(RoleLevel.ProjectManager)}")]
    public IActionResult DeepDelete(Guid guid)
    {
        var delete = _assignmentServices.DeleteDeepAssignment(guid);
        if (delete is -1) return NotFound(new ResponseHandlers<AssignmentDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });
        return Ok(new ResponseHandlers<AssignmentDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data was Found"
        });
    }



    //Basic CRUD
    [HttpGet]
    [Authorize]
    public IActionResult GetAll()
    {
        var entities = _assignmentServices.Get();
        if (!entities.Any())
        {
            return NotFound(new ResponseHandlers<AssignmentByManagerDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        return Ok(new ResponseHandlers<IEnumerable<AssignmentByManagerDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Found",
            Data = entities
        });
    }

    [HttpGet("{guid}")]
    [Authorize]
    public IActionResult Get(Guid guid)
    {
        var entity = _assignmentServices.Get(guid);
        if (entity == null) return NotFound(new ResponseHandlers<AssignmentByManagerDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<AssignmentByManagerDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Found",
            Data = entity
        });
    }

    [HttpGet("Manager/{guid}")]
    [Authorize]
    public IActionResult GetByManagerId(Guid guid)
    {
        var entity = _assignmentServices.GetByManager(guid);
        if (entity == null) return NotFound(new ResponseHandlers<AssignmentByManagerDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<IEnumerable<AssignmentByManagerDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.Found.ToString(),
            Message = "Data Found",
            Data = entity
        });
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(RoleLevel.ProjectManager)}")]
    public IActionResult Create(NewAssignmentDto entity)
    {
        var created = _assignmentServices.Create(entity);
        if (created == null) return NotFound(new ResponseHandlers<AssignmentDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<AssignmentDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data has been created",
            Data = created
        });
    }

    [HttpPut]
    [Authorize(Roles = $"{nameof(RoleLevel.ProjectManager)}")]
    public IActionResult Update(UpdateAssignmentDto entity)
    {
        var updated = _assignmentServices.Update(entity);
        if (updated is -1) return NotFound(new ResponseHandlers<UpdateAssignmentDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<UpdateAssignmentDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data has been updated"
        });
    }

    [HttpDelete]
    [Authorize(Roles = $"{nameof(RoleLevel.ProjectManager)}")]
    public IActionResult Delete(Guid guid)
    {
        var delete = _assignmentServices.Delete(guid);
        if (delete is -1) return NotFound(new ResponseHandlers<AssignmentDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });
        return Ok(new ResponseHandlers<AssignmentDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data has been deleted",
        });
    }
    //==========
}
