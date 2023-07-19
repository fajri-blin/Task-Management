using ClientSide.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_Management.DTOs.AccountDto;

namespace ClientSide.Controllers;

[Controller]
public class AccountController : Controller
{
    private readonly IAccountRepository _accountRepository;

    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(RegisterDto registerDto)
    {
        var result = await _accountRepository.Register(registerDto);
        if (result == null)
        {
            return RedirectToAction("Error","Index");
        }else if(result.Code == 200)
        {
            return View("Index");
        }
        return View();
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(LoginDto loginDto)
    {
        var result = await _accountRepository.Login(loginDto);
        if (result == null)
        {
            return RedirectToAction("Error", "Home");
        }
        else if (result.Code == 404)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View();
        }
        else if (result.Code == 200)
        {
            HttpContext.Session.SetString("JWToken", result.Data);
            return RedirectToAction("Index", "Home");
        }
        return View();
    }
}
