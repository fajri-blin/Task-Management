using Microsoft.AspNetCore.Mvc;
using System.Net;
using Task_Management.DTOs.AccountProgressDto;
using Task_Management.Service;
using Task_Management.Utilities.Handler;

namespace Task_Management.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = $"{nameof(RoleLevel.Developer)}")]
public class AccountProgressController : ControllerBase
{
    private readonly AccountProgressService _accountProgressSevices;

    public AccountProgressController(AccountProgressService accountSevices)
    {
        _accountProgressSevices = accountSevices;
    }

    [HttpGet("GetByProgress/{guid}")]
    public IActionResult GetByProgress(Guid guid)
    {
        var entities = _accountProgressSevices.GetByProgressGuid(guid);
        if (entities == null) return NotFound(new ResponseHandlers<AccountProgressDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<IEnumerable<AccountProgressDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entities
        });
    }

    [HttpGet("GetByAccountGuid/{guid}")]
    public IActionResult GetByAccountGuid(Guid guid)
    {
        var entities = _accountProgressSevices.GetByAccountGuid(guid);
        if (entities == null) return NotFound(new ResponseHandlers<AccountProgressDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<IEnumerable<AccountProgressDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entities
        });
    }



    //Basic CRUD
    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _accountProgressSevices.Get();
        if (entities == null)
        {
            return NotFound(new ResponseHandlers<AccountProgressDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        return Ok(new ResponseHandlers<IEnumerable<AccountProgressDto>>
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
        var entity = _accountProgressSevices.Get(guid);
        if (entity == null) return NotFound(new ResponseHandlers<AccountProgressDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<AccountProgressDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entity
        });
    }

    [HttpPost]
    public IActionResult Create(NewAccountProgressDto entity)
    {
        var created = _accountProgressSevices.Create(entity);
        if (created == null) return NotFound(new ResponseHandlers<AccountProgressDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Failed to created"
        });

        return Ok(new ResponseHandlers<AccountProgressDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Successfully created",
            Data = created
        });
    }

    [HttpPut]
    public IActionResult Update(AccountProgressDto entity)
    {
        var updated = _accountProgressSevices.Update(entity);
        if (updated is -1) return NotFound(new ResponseHandlers<int>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<int>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data has been updated"
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _accountProgressSevices.Delete(guid);
        if (delete is -1) return NotFound(new ResponseHandlers<int>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });
        return Ok(new ResponseHandlers<int>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data failed to deleted"
        });
    }
    //==========
}
