using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_Management.DTOs.RoleDto;
using Task_Management.Service;
using Task_Management.Utilities.Enum;

namespace Task_Management.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = $"{nameof(RoleLevel.Developer)}")]
public class RoleController : ControllerBase
{
    private readonly RoleService _roleServices;

    public RoleController(RoleService accountSevices)
    {
        _roleServices = accountSevices;
    }

    //Basic CRUD
    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _roleServices.Get();
        if (entities == null)
        {
            return NotFound();
        }
        return Ok(entities);
    }

    [HttpGet("{guid}")]
    public IActionResult Get(Guid guid) 
    {
        var entity = _roleServices.Get(guid);
        if (entity == null) return NotFound();

        return Ok(entity);
    }

    [HttpPost]
    public IActionResult Create(NewRoleDto entity)
    {
        var created = _roleServices.Create(entity);
        if (created == null) return NotFound();
        
        return Ok(created);
    }

    [HttpPut]
    public IActionResult Update(RoleDto entity) 
    {
        var updated = _roleServices.Update(entity);
        if(updated is -1) return NotFound();

        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _roleServices.Delete(guid);
        if (delete is -1) return NotFound();
        return Ok(delete);
    }
    //==========
}
