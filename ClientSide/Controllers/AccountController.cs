using ClientSide.Contract;
using ClientSide.Utilities.Enum;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Account;
using ClientSide.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ClientSide.Controllers;

[Controller]
public class AccountController : Controller
{
    private readonly IAccountRepository _accountRepository;

    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _accountRepository.Get();
        var components = new ComponentHandlers
        {
            Footer = false,
            SideBar = true,
            Navbar = true,
        };

        var accounts = result.Data.Select(entity => new AccountVM
        {
            Guid = entity.Guid,
            Name = entity.Name,
            Role = entity.Role,
            IsDeleted = entity.IsDeleted,
        }).ToList();
        ViewBag.Components = components;
        return View("Index", accounts);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var account = await _accountRepository.Get(Guid.Parse(HttpContext.User.FindFirstValue("Guid")));
        var profile = new GetProfileVM
        {
            Guid = account.Guid,
            Username = account.Username,
            Email = account.Email,
            Name = account.Name,
        };
        var components = new ComponentHandlers
        {
            Footer = false,
            SideBar = true,
            Navbar = true,
        };
        ViewBag.Components = components;
        return View("Profile", profile);
    }

    [Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
    [HttpPost]
    public async Task<IActionResult> Delete(Guid guid)
    {
        var result = await _accountRepository.SoftDelete(guid);
        if (result.Code == 404)
        {
            return Json(new { code = 404, message = "Account not found" });
        }
        else if (result.Code == 200)
        {
            return Json(new { code = 200, message = "Account deleted successfully" });
        }
        else
        {
            return Json(new { code = result.Code, message = result.Message });
        }
    }

    [Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
    [HttpPost]
    public async Task<IActionResult> Activate(Guid guid)
    {
        var result = await _accountRepository.Activation(guid);
        if (result.Code == 404)
        {
            return Json(new { code = 404, message = "Account not found" });
        }
        else if (result.Code == 200)
        {
            return Json(new { code = 200, message = "Account activated successfully" });
        }
        else
        {
            return Json(new { code = result.Code, message = result.Message });
        }
    }

    [Authorize]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Profile([FromForm] GetProfileVM updateProfileVM)
    {
        var result = await _accountRepository.UpdateProfile(updateProfileVM);
        if (result.Errors != null)
        {
            if (result.Errors is Dictionary<string, List<string>> errorDictionary)
            {
                foreach (var key in errorDictionary.Keys)
                {
                    foreach (var errorMessage in errorDictionary[key])
                    {
                        ModelState.AddModelError(key, errorMessage);
                    }
                }
            }
            var components = new ComponentHandlers
            {
                Footer = false,
                SideBar = true,
                Navbar = true,
            };
            ViewBag.Components = components;
            string errorsJson = JsonConvert.SerializeObject(result.Errors);
            TempData["Error"] = errorsJson;

            return View("Profile");
        }
        return RedirectToAction("Profile");
    }

    [Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateAccount([FromForm] UpdateAccountVM updateVM)
    {
        var result = await _accountRepository.Update(updateVM);
        if (result.Errors != null)
        {
            if (result.Errors is Dictionary<string, List<string>> errorDictionary)
            {
                foreach (var key in errorDictionary.Keys)
                {
                    foreach (var errorMessage in errorDictionary[key])
                    {
                        TempData[$"{key}_Error"] = errorMessage;
                    }
                }
            }

            return RedirectToAction("Edit", new { guid = updateVM.Guid });
        }
        return RedirectToAction("Index");

    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPass(ForgotPasswordVM forgotPasswordVM)
    {
        var result = await _accountRepository.ForgotPassword(forgotPasswordVM);
        if (result.Errors != null)
        {
            if (result.Errors is Dictionary<string, List<string>> errorDictionary)
            {
                foreach (var key in errorDictionary.Keys)
                {
                    foreach (var errorMessage in errorDictionary[key])
                    {
                        ModelState.AddModelError(key, errorMessage);
                    }
                }
            }
            string errorsJson = JsonConvert.SerializeObject(result.Errors);
            TempData["Error"] = errorsJson;
            return View();
        }

        // If the request is successful, redirect to the CheckAccountOTP action
        return RedirectToAction("CheckAccountOTP", new { email = forgotPasswordVM.Email });
    }
    [AllowAnonymous]
    [HttpGet]
    public IActionResult ForgotPass()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CheckAccountOTP(CheckOTPVM checkOTPVM)
    {
        var result = await _accountRepository.CheckAccountOTP(checkOTPVM);
        if (result.Code == 404)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction("CheckAccountOTP", new { email = checkOTPVM.Email });
        }
        else if (result is null)
        {
            return RedirectToAction("Error", "Index");
        }

        // Store the OTP value in TempData to pass it to ChangeAccountPassword action

        return RedirectToAction("ChangeAccountPasswords", new { email = checkOTPVM.Email, otp = checkOTPVM.OTP });
    }
    [AllowAnonymous]
    [HttpGet]
    public IActionResult CheckAccountOTP(string email)
    {
        var viewModel = new CheckOTPVM
        {
            Email = email
        };

        return View(viewModel);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> ChangeAccountPassword(ChangePasswordVM changePasswordVM)
    {
        var result = await _accountRepository.ChangeAccountPassword(changePasswordVM);
        if (result == null || result.Code != 200)
        {
            if (result.Errors != null)
            {
                if (result.Errors is Dictionary<string, List<string>> errorDictionary)
                {
                    foreach (var key in errorDictionary.Keys)
                    {
                        foreach (var errorMessage in errorDictionary[key])
                        {
                            TempData[$"{key}_Error"] = errorMessage;
                        }
                    }
                }
            }
            TempData["Error"] = result.Message;
            return RedirectToAction("ChangeAccountPasswords", new { email = changePasswordVM.Email, otp = changePasswordVM.OTP });
        }

        // If the password change is successful, redirect to the login page or another appropriate page.
        return RedirectToAction("SignIn");
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult ChangeAccountPasswords(string email, int otp)
    {
        // Check if the OTP value is not null and can be parsed to int
        if (otp != null && int.TryParse(otp.ToString(), out int otpValue))
        {
            // Pass the email and OTP as parameters to the view model.
            var viewModel = new ChangePasswordVM
            {
                Email = email,
                OTP = otpValue // Set the OTP value in the view model
            };

            return View("ChangeAccountPassword", viewModel);
        }
        else
        {
            // Handle the case when OTP value is not available or cannot be parsed to int
            // You can choose to redirect to an error page or take appropriate action.
            return RedirectToAction("Error", "Index");
        }
    }



    [Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> SignUp(RegisterVM registerDto)
    {
        var result = await _accountRepository.Register(registerDto);
        if (result == null)
        {
            return RedirectToAction("Error", "Index");
        }
        if (result.Errors != null)
        {
            if (result.Errors is Dictionary<string, List<string>> errorDictionary)
            {
                foreach (var key in errorDictionary.Keys)
                {
                    foreach (var errorMessage in errorDictionary[key])
                    {
                        ModelState.AddModelError(key, errorMessage);
                    }
                }
            }
            TempData["Error"] = result.Errors;
            return View();
        }
        else if (result.Code == 200)
        {
            return RedirectToAction("Index");
        }
        return View();
    }
    [Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }

    [AllowAnonymous]
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
            TempData["Error"] = $"{result.Message}!";
            return View();
        }
        else if (result.Code == 200)
        {
            HttpContext.Session.SetString("JWToken", result.Data);
            return RedirectToAction("Index", "Dashboard");
        }

        return View();
    }
    [AllowAnonymous]
    [HttpGet]
    public IActionResult SignIn()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Dashboard");
        }

        return View();
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    [Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
    [HttpGet]
    public async Task<IActionResult> Edit(Guid guid)
    {

        var account = await _accountRepository.Get(guid);
        var updateVM = new UpdateAccountVM
        {
            Guid = account.Guid,
            Username = account.Username,
            Email = account.Email,
            Name = account.Name,
            Role = account.Role,
        };

        return View("Edit", updateVM);
    }
}
