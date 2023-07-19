﻿using Microsoft.AspNetCore.Mvc;
using Task_Management.Service;
using Task_Management.DTOs.AccountRoleDto;
using Microsoft.AspNetCore.Authorization;
using Task_Management.Utilities.Enum;

namespace Task_Management.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = $"{nameof(RoleLevel.Developer)}")]
public class AccountRoleController : ControllerBase
{
    private readonly AccountRoleService _accountRoleSevices;

    public AccountRoleController(AccountRoleService accountSevices)
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
            return NotFound();
        }
        return Ok(entities);
    }

    [HttpGet("{guid}")]
    public IActionResult Get(Guid guid) 
    {
        var entity = _accountRoleSevices.Get(guid);
        if (entity == null) return NotFound();

        return Ok(entity);
    }

    [HttpPost]
    public IActionResult Create(NewAccountRoleDto entity)
    {
        var created = _accountRoleSevices.Create(entity);
        if (created == null) return NotFound();
        
        return Ok(created);
    }

    [HttpPut]
    public IActionResult Update(AccountRoleDto entity) 
    {
        var updated = _accountRoleSevices.Update(entity);
        if(updated is -1) return NotFound();

        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _accountRoleSevices.Delete(guid);
        if (delete is -1) return NotFound();
        return Ok(delete);
    }
    //==========
}