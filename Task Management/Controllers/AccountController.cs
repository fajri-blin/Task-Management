using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Task_Management.Dtos.AccountDto;
using Task_Management.DTOs.AccountDto;
using Task_Management.Service;
using Task_Management.Utilities.Handler;

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

    [HttpPost("ForgotPassword")]
    public IActionResult ForgotPassword(ForgotPasswordDto forgotPassword)
    {
        var isUpdated = _accountSevices.ForgotPassword(forgotPassword);
        if (isUpdated == 0)
            return NotFound(new ResponseHandlers<AccountDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Email not found"
            });

        if (isUpdated is -1)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandlers<AccountDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving data from the database"
            });

        return Ok(new ResponseHandlers<AccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Otp has been sent to your email"
        });
    }

    [HttpPost("CheckOtp")]
    public IActionResult CheckOtp(CheckOtp checkOtp)
    {
        var isUpdated = _accountSevices.CheckOtp(checkOtp);
        if (isUpdated == 0)
            return NotFound(new ResponseHandlers<AccountDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Otp doesn't match"
            });
        if (isUpdated == 1)
        {
            return NotFound(new ResponseHandlers<ChangePasswordDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Otp alredy expired"
            });
        }
        return Ok(new ResponseHandlers<AccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Otp match"
        });
    }

    [HttpPut("changePassword")]
    public IActionResult Update(ChangePasswordDto changePasswordDto)
    {
        var update = _accountSevices.ChangePassword(changePasswordDto);
        if (update is -1)
        {
            return NotFound(new ResponseHandlers<ChangePasswordDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Email not Found"
            });
        }
        if (update is 0)
        {
            return NotFound(new ResponseHandlers<ChangePasswordDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Otp doesn't match"
            });
        }
        if (update is 1)
        {
            return NotFound(new ResponseHandlers<ChangePasswordDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Otp has been used"
            });
        }
        if (update is 2)
        {
            return NotFound(new ResponseHandlers<ChangePasswordDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Otp alredy expired"
            });
        }
        return Ok(new ResponseHandlers<ChangePasswordDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully updated"
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
        return Ok(new ResponseHandlers<IEnumerable<AccountDto>>
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
        if (updated is -1) return NotFound(new ResponseHandlers<AccountDto>
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
