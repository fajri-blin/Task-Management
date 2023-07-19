using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_Management.DTOs.AssignMapDto;
using Task_Management.Service;
using Task_Management.Utilities.Enum;

namespace Task_Management.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{nameof(RoleLevel.Developer)}")]
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
            return NotFound();
        }
        return Ok(entities);
    }

    [HttpGet("{guid}")]
    public IActionResult Get(Guid guid) 
    {
        var entity = _assignmentServices.Get(guid);
        if (entity == null) return NotFound();

        return Ok(entity);
    }

    [HttpPost]
    public IActionResult Create(NewAssignMapDto entity)
    {
        var created = _assignmentServices.Create(entity);
        if (created == null) return NotFound();
        
        return Ok(created);
    }

    [HttpPut]
    public IActionResult Update(AssignMapDto entity) 
    {
        var updated = _assignmentServices.Update(entity);
        if(updated is -1) return NotFound();

        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _assignmentServices.Delete(guid);
        if (delete is -1) return NotFound();
        return Ok(delete);
    }
    //==========
}
