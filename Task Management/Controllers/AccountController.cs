using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Task_Management.Dtos.AccountDto;
using Task_Management.DTOs.AccountDto;
using Task_Management.Service;
using Task_Management.Utilities.Enum;
using Task_Management.Utilities.Handler;

namespace Task_Management.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountSevices;

    public AccountController(AccountService accountSevices)
    {
        _accountSevices = accountSevices;
    }

    [HttpPost("Register")]
    [Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
    public IActionResult Register(RegisterDto registerDto)
    {
        var registerResult = _accountSevices.Register(registerDto);
        if (registerResult == null)
        {
            return NotFound(new ResponseHandlers<DetailAccountDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Register Failed"
            });
        }
        return Ok(new ResponseHandlers<DetailAccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Register Successfully",
            Data = registerResult
        });
    }


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
                    Message = "Account has been deactivated"
                });
            case "-1":
                return NotFound(new ResponseHandlers<AccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Login Failed"
                });
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
    public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
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

    [HttpGet("Photo/{guid}")]
    public IActionResult GetPhoto(Guid guid)
    {
        var fileResult = _accountSevices.Photo(guid);
        if (fileResult == null)
        {
            return NotFound(new ResponseHandlers<string>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        return fileResult;
    }

    [HttpPut("Activation")]
    [Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
    public IActionResult Activation(GetGuidDto guid)
    {
        var activate = _accountSevices.Activation(guid.Guid);
        if (activate is -1) return NotFound(new ResponseHandlers<AccountDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });
        return Ok(new ResponseHandlers<AccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Successfully activated",
        });
    }

    //Basic CRUD
    [HttpGet]
    [Authorize]
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
    [Authorize]
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

    [HttpPut]
    [Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
    public async Task<IActionResult> Update([FromForm] UpdateAccountDto entity)
    {
        var updated = await _accountSevices.Update(entity);
        if (updated is 0) return NotFound(new ResponseHandlers<UpdateAccountDto>
        {
            Code = StatusCodes.Status400BadRequest,
            Status = HttpStatusCode.BadRequest.ToString(),
            Message = "Failed Update Account"
        });

        return Ok(new ResponseHandlers<UpdateAccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data has been Updated",
        });
    }

    [HttpPut("Profile/Update")]
    [Authorize]
    public async Task<IActionResult> ProfileUpdate([FromForm] UpdateAccountDto entity)
    {
        var updated = await _accountSevices.ProfileUpdate(entity);
        if (updated is 0) return NotFound(new ResponseHandlers<UpdateAccountDto>
        {
            Code = StatusCodes.Status400BadRequest,
            Status = HttpStatusCode.BadRequest.ToString(),
            Message = "Failed Update Profile"
        });

        return Ok(new ResponseHandlers<UpdateAccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data has been Updated",
        });
    }

    [HttpPut("SoftDelete")]
    [Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
    public IActionResult Delete(GetGuidDto guid)
    {
        var delete = _accountSevices.Delete(guid.Guid);
        if (delete is -1) return NotFound(new ResponseHandlers<AccountDto>
        {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Data Not Found"
        });
        if (delete is 0) return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandlers<AccountDto>
        {
            Code = StatusCodes.Status500InternalServerError,
            Status = HttpStatusCode.InternalServerError.ToString(),
            Message = "Error retrieving data from the database"
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
