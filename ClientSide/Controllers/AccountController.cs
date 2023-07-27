using ClientSide.Contract;
using ClientSide.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> ForgotPass(ForgotPasswordVM forgotPasswordVM)
    {
        var result = await _accountRepository.ForgotPassword(forgotPasswordVM);
        if (result == null)
        {
            return RedirectToAction("Error", "Index");
        }
        else if (result.Code == 404)
        {
            return View("Index");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(RegisterVM registerDto)
    {
        var result = await _accountRepository.Register(registerDto);
        if (result == null)
        {
            return RedirectToAction("Error", "Index");
        }
        else if (result.Code == 200)
        {
            return View("Index");
        }
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> SignIn(SignInVM signInDto)
    {
        var result = await _accountRepository.Login(signInDto);
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

    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpGet]
    public IActionResult ForgotPass()
    {
        return View();
    }

    [HttpGet]
    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
