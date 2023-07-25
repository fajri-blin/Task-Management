using Microsoft.AspNetCore.Mvc;
using System.Net;
using Task_Management.DTOs.AssignMapDto;
using Task_Management.Service;
using Task_Management.Utilities.Handler;

namespace Task_Management.Controllers;

[ApiController]
[Route("api/[controller]")]
/*[Authorize(Roles = $"{nameof(RoleLevel.Developer)}")]*/
public class AssignMapController : ControllerBase
{
    private readonly AssignMapService _assignmentServices;

    public AssignMapController(AssignMapService accountSevices)
    {
        _assignmentServices = accountSevices;
    }

    //Basic CRUD
    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _assignmentServices.Get();
        if (entities == null)
        {
            return NotFound(new ResponseHandlers<AssignMapDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        return Ok(new ResponseHandlers<IEnumerable<AssignMapDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entities
        });
    }

    [HttpGet("{guid}")]
    public IActionResult Get(Guid guid)
    {
        var entity = _assignmentServices.Get(guid);
        if (entity == null) return NotFound(new ResponseHandlers<AssignMapDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<AssignMapDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entity
        });
    }

    [HttpPost]
    public IActionResult Create(NewAssignMapDto entity)
    {
        var created = _assignmentServices.Create(entity);
        if (created == null) return NotFound(new ResponseHandlers<AssignMapDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<AssignMapDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Not Found",
            Data = created
        });
    }

    [HttpPut]
    public IActionResult Update(AssignMapDto entity)
    {
        var updated = _assignmentServices.Update(entity);
        if (updated is -1) return NotFound(new ResponseHandlers<AssignMapDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<AssignMapDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data has been updated"
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _assignmentServices.Delete(guid);
        if (delete is -1) return NotFound(new ResponseHandlers<AssignMapDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });
        return Ok(new ResponseHandlers<AssignMapDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data has been deleted"
        });
    }
    //==========
}
