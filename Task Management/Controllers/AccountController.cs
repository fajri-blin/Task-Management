using Microsoft.AspNetCore.Mvc;
using Task_Management.Service;
using Task_Management.DTOs.AccountDto;
using System.Net;
using Task_Management.Utilities.Handler;
using Microsoft.AspNetCore.Authorization;
using Task_Management.Utilities.Enum;

namespace Task_Management.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = $"{nameof(RoleLevel.Developer)}")]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountSevices;

    public AccountController(AccountService accountSevices)
    {
        _accountSevices = accountSevices;
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public IActionResult Register(RegisterDto registerDto)
    {
        var registerResult = _accountSevices.Register(registerDto);
        if (registerResult == null)
        {
            return NotFound(new ResponseHandlers<RegisterDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Register Failed"
            });
        }
        return Ok(new ResponseHandlers<RegisterDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Register Successfully",
            Data = registerResult
        });
    }


    [AllowAnonymous]
    [HttpPost("Login")]
    public IActionResult Login(LoginDto loginDto)
    {
        var loginResult = _accountSevices.Login(loginDto);
        switch (loginResult)
        {
            case "0":
                return NotFound(new ResponseHandlers<AccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Login Failed"
                });
                break;
            case "-1":
                return NotFound(new ResponseHandlers<AccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Login Failed"
                });
                break;
            case "-2":
                return NotFound(new ResponseHandlers<AccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Login Failed"
                });
                break;
        }
        return Ok(new ResponseHandlers<string>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Login Successfully",
            Data = loginResult
        });

    }

    //Basic CRUD
    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _accountSevices.Get();
        if (entities == null)
        {
            return NotFound(new ResponseHandlers<AccountDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        return Ok(new ResponseHandlers<AccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = (AccountDto)entities
        });
    }

    [HttpGet("{guid}")]
    public IActionResult Get(Guid guid) 
    {
        var entity = _accountSevices.Get(guid);
        if (entity == null) return NotFound(new ResponseHandlers<AccountDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(entity);
    }

    [HttpPost]
    public IActionResult Create(NewAccountDto entity)
    {
        var created = _accountSevices.Create(entity);
        if (created == null) return NotFound(new ResponseHandlers<AccountDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });
        
        return Ok(new ResponseHandlers<AccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Successfully created",
            Data = (AccountDto)created
        });
    }

    [HttpPut]
    public IActionResult Update(AccountDto entity) 
    {
        var updated = _accountSevices.Update(entity);
        if(updated is -1) return NotFound(new ResponseHandlers<AccountDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });

        return Ok(new ResponseHandlers<AccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data has been Updated",
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _accountSevices.Delete(guid);
        if (delete is -1) return NotFound(new ResponseHandlers<AccountDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });
        return Ok(new ResponseHandlers<AccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Successfully deleted",
        });
    }
    //==========
}
