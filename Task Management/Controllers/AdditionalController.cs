using Microsoft.AspNetCore.Mvc;
using Task_Management.Service;
using Task_Management.DTOs.AdditionalDto;
using Task_Management.DTOs.NewAdditionalDto;
using System.Net;
using Task_Management.DTOs.AccountRoleDto;
using Task_Management.Utilities.Handler;

namespace Task_Management.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = $"{nameof(RoleLevel.Developer)}")]
public class AdditionalController : ControllerBase
{
    private readonly AdditionalService _accountRoleSevices;

    public AdditionalController(AdditionalService accountSevices)
    {
        _accountRoleSevices = accountSevices;
    }

    //Basic CRUD
    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _accountRoleSevices.Get();
        if (entities == null)
        {
            return NotFound(new ResponseHandlers<AdditionalDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        return Ok(new ResponseHandlers<AdditionalDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = (AdditionalDto)entities
        });
    }

    [HttpGet("{guid}")]
    public IActionResult Get(Guid guid) 
    {
        var entity = _accountRoleSevices.Get(guid);
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
    public IActionResult Create(NewAdditionalDto entity)
    {
        var created = _accountRoleSevices.Create(entity);
        if (created == null) return NotFound(new ResponseHandlers<AccountRoleDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });
        
        return Ok(new ResponseHandlers<AdditionalDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data created",
            Data = created
        });
    }

    [HttpPut]
    public IActionResult Update(AdditionalDto entity) 
    {
        var updated = _accountRoleSevices.Update(entity);
        if(updated is -1) return NotFound(new ResponseHandlers<int>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<int>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Updated",
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _accountRoleSevices.Delete(guid);
        if (delete is -1) return NotFound(new ResponseHandlers<int>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });
        return Ok(new ResponseHandlers<int>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Not Found"
        });
    }
    //==========
}
