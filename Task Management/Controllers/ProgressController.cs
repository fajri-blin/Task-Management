using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_Management.DTOs.ProgressDto;
using Task_Management.Service;
using Task_Management.Utilities.Enum;

namespace Task_Management.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{nameof(RoleLevel.Developer)}")]
public class ProgressController : ControllerBase
{
    private readonly ProgressService _progressServices;

    public ProgressController(ProgressService accountSevices)
    {
        _progressServices = accountSevices;
    }

    //Basic CRUD
    //[HttpGet]
    //public IActionResult GetAll()
    //{
    //    var entities = _progressServices.Get();
    //    if (entities == null)
    //    {
    //        return NotFound();
    //    }
    //    return Ok(entities);
    //}

    //[HttpGet("{guid}")]
    //public IActionResult Get(Guid guid) 
    //{
    //    var entity = _progressServices.Get(guid);
    //    if (entity == null) return NotFound();

    //    return Ok(entity);
    //}

    //[HttpPost]
    //public IActionResult Create(NewProgressDto entity)
    //{
    //    var created = _progressServices.Create(entity);
    //    if (created == null) return NotFound();
        
    //    return Ok(created);
    //}

    //[HttpPut]
    //public IActionResult Update(ProgressDto entity) 
    //{
    //    var updated = _progressServices.Update(entity);
    //    if(updated is -1) return NotFound();

    //    return Ok();
    //}

    //[HttpDelete]
    //public IActionResult Delete(Guid guid)
    //{
    //    var delete = _progressServices.Delete(guid);
    //    if (delete is -1) return NotFound();
    //    return Ok(delete);
    //}
    //==========
}
