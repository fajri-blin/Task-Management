using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Task_Management.DTOs.AdditionalDto;
using Task_Management.DTOs.NewAdditionalDto;
using Task_Management.Service;
using Task_Management.Utilities.Enum;
using Task_Management.Utilities.Handler;

namespace Task_Management.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{nameof(RoleLevel.ProjectManager)}, {nameof(RoleLevel.Staff)}")]
public class AdditionalController : ControllerBase
{
    private readonly AdditionalService _additionalSevices;

    public AdditionalController(AdditionalService accountSevices)
    {
        _additionalSevices = accountSevices;
    }

    [HttpGet("GetByProgressKey/{guid}")]
    public IActionResult GetByProgressKey(Guid guid)
    {
        var entities = _additionalSevices.GetByProgressGuid(guid);
        if (entities == null) return NotFound(new ResponseHandlers<AdditionalDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<IEnumerable<AdditionalDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entities
        });
    }

    [HttpGet("Download")]
    public IActionResult GetFile(Guid guid)
    {
        var fileResult = _additionalSevices.DownloadFile(guid);
        if (fileResult == null)
        {
            return NotFound(new ResponseHandlers<AdditionalDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        return fileResult;
    }

    //Basic CRUD
    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _additionalSevices.Get();
        if (entities == null)
        {
            return NotFound(new ResponseHandlers<AdditionalDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        return Ok(new ResponseHandlers<IEnumerable<AdditionalDto>>
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
        var entity = _additionalSevices.Get(guid);
        if (entity == null) return NotFound(new ResponseHandlers<AdditionalDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<AdditionalDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entity
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] NewAdditionalDto entity)
    {
        var created = await _additionalSevices.Create(entity);
        if (created == null) return NotFound(new ResponseHandlers<NewAdditionalDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<IEnumerable<AdditionalDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data created",
            Data = created
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _additionalSevices.Delete(guid);
        if (delete is -1) return NotFound(new ResponseHandlers<string>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });
        return Ok(new ResponseHandlers<string>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Deleted"
        });
    }
    //==========
}
