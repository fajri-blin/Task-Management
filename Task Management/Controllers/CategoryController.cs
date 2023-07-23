using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Task_Management.DTOs.AccountRoleDto;
using Task_Management.DTOs.CategoryDto;
using Task_Management.Service;
using Task_Management.Utilities.Enum;
using Task_Management.Utilities.Handler;

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
            return NotFound(new ResponseHandlers<CategoryDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        return Ok(new ResponseHandlers<IEnumerable<CategoryDto>>
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
        var entity = _cateogoryServices.Get(guid);
        if (entity == null) return NotFound(new ResponseHandlers<CategoryDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<CategoryDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data= entity
        });
    }

    [HttpPost]
    public IActionResult Create(NewCategoryDto entity)
    {
        var created = _cateogoryServices.Create(entity);
        if (created == null) return NotFound(new ResponseHandlers<CategoryDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });
        
        return Ok(new ResponseHandlers<CategoryDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data= created
        });
    }

    [HttpPut]
    public IActionResult Update(CategoryDto entity) 
    {
        var updated = _cateogoryServices.Update(entity);
        if(updated is -1) return NotFound(new ResponseHandlers<CategoryDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<CategoryDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data has been updated"
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _cateogoryServices.Delete(guid);
        if (delete is -1) return NotFound(new ResponseHandlers<CategoryDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });
        return Ok(new ResponseHandlers<CategoryDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data has been deleted"
        });
    }
    //==========
}
