using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_Management.DTOs.CategoryDto;
using Task_Management.Service;
using Task_Management.Utilities.Enum;

namespace Task_Management.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{nameof(RoleLevel.Developer)}")]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _cateogoryServices;

    public CategoryController(CategoryService accountSevices)
    {
        _cateogoryServices = accountSevices;
    }

    //Basic CRUD
    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _cateogoryServices.Get();
        if (entities == null)
        {
            return NotFound();
        }
        return Ok(entities);
    }

    [HttpGet("{guid}")]
    public IActionResult Get(Guid guid) 
    {
        var entity = _cateogoryServices.Get(guid);
        if (entity == null) return NotFound();

        return Ok(entity);
    }

    [HttpPost]
    public IActionResult Create(NewCategoryDto entity)
    {
        var created = _cateogoryServices.Create(entity);
        if (created == null) return NotFound();
        
        return Ok(created);
    }

    [HttpPut]
    public IActionResult Update(CategoryDto entity) 
    {
        var updated = _cateogoryServices.Update(entity);
        if(updated is -1) return NotFound();

        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _cateogoryServices.Delete(guid);
        if (delete is -1) return NotFound();
        return Ok(delete);
    }
    //==========
}
