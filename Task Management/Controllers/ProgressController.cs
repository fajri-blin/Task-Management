using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Task_Management.Dtos.ProgressDto;
using Task_Management.DTOs.ProgressDto;
using Task_Management.Service;
using Task_Management.Utilities.Enum;
using Task_Management.Utilities.Handler;

namespace Task_Management.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProgressController : ControllerBase
{
    private readonly ProgressService _progressServices;

    public ProgressController(ProgressService accountSevices)
    {
        _progressServices = accountSevices;
    }

    [HttpGet("GetByAssignmentKey/{guid}")]
    [Authorize(Roles = $"{nameof(RoleLevel.ProjectManager)}, {nameof(RoleLevel.Staff)}")]
    public IActionResult GetByAssigmentKey(Guid guid)
    {
        var entities = _progressServices.GetByAssignmentGuid(guid);
        if (entities == null) return NotFound(new ResponseHandlers<ProgressDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<IEnumerable<ProgressDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entities
        });
    }

    [HttpDelete("DeepDelete/{guid}")]
    [Authorize(Roles = $"{nameof(RoleLevel.ProjectManager)}")]
    public IActionResult DeepDelete(Guid guid)
    {
        var delete = _progressServices.DeleteDeepProgress(guid);
        if (delete is -1) return NotFound(new ResponseHandlers<ProgressDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });
        return Ok(new ResponseHandlers<ProgressDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data has been deleted"
        });
    }

    //Basic CRUD
    [HttpGet]
    [Authorize(Roles = $"{nameof(RoleLevel.ProjectManager)}, {nameof(RoleLevel.Staff)}")]
    public IActionResult GetAll()
    {
        var entities = _progressServices.Get();
        if (entities == null)
        {
            return NotFound(new ResponseHandlers<ProgressDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        return Ok(new ResponseHandlers<IEnumerable<ProgressDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entities
        });
    }

    [HttpGet("{guid}")]
    [Authorize(Roles = $"{nameof(RoleLevel.ProjectManager)}, {nameof(RoleLevel.Staff)}")]
    public IActionResult Get(Guid guid)
    {
        var entity = _progressServices.Get(guid);
        if (entity == null) return NotFound(new ResponseHandlers<ProgressDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<ProgressDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entity
        });
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(RoleLevel.ProjectManager)}")]
    public IActionResult Create(NewProgressDto entity)
    {
        var created = _progressServices.Create(entity);
        if (created == null) return NotFound(new ResponseHandlers<ProgressDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<ProgressDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data has been created",
            Data = created
        });
    }

    [HttpPut]
    [Authorize(Roles = $"{nameof(RoleLevel.ProjectManager)}, {nameof(RoleLevel.Staff)}")]
    public IActionResult Update(ProgressDto entity)
    {
        var updated = _progressServices.Update(entity);
        if (updated is -1) return NotFound(new ResponseHandlers<ProgressDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<ProgressDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data has been updated"
        });
    }

    [HttpPut("Status")]
    [Authorize(Roles = $"{nameof(RoleLevel.ProjectManager)}, {nameof(RoleLevel.Staff)}")]
    public IActionResult UpdateStatus(UpdateStatusDto entity)
    {
        var updated = _progressServices.UpdateStatus(entity);
        if (updated is 0) return NotFound(new ResponseHandlers<UpdateStatusDto>
        {
            Code = StatusCodes.Status406NotAcceptable,
            Status = HttpStatusCode.NotAcceptable.ToString(),
            Message = "Data Failed Update"
        });

        return Ok(new ResponseHandlers<UpdateStatusDto>
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
        var delete = _progressServices.Delete(guid);
        if (delete is -1) return NotFound(new ResponseHandlers<ProgressDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });
        return Ok(new ResponseHandlers<ProgressDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data has been deleted",
        });
    }

}
