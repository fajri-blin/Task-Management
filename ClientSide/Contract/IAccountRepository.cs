using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Account;
using ClientSide.ViewModels.Profile;
using Microsoft.AspNetCore.Mvc;

namespace ClientSide.Contract;

public interface IAccountRepository : IGeneralRepository<AccountVM>
{
    Task<ResponseHandlers<string>> Login(SignInVM signInDto);
    Task<ResponseHandlers<RegisterVM>> Register(RegisterVM registerDto);
    Task<ResponseHandlers<ForgotPasswordVM>> ForgotPassword(ForgotPasswordVM forgotPasswordVM);
    Task<ResponseHandlers<CheckOTPVM>> CheckAccountOTP(CheckOTPVM checkOTPVM);
    Task<ResponseHandlers<ChangePasswordVM>> ChangeAccountPassword(ChangePasswordVM changePasswordVM);
    Task<ResponseHandlers<UpdateAccountVM>> Update([FromForm] UpdateAccountVM updateVM);
    Task<ResponseHandlers<GetProfileVM>> UpdateProfile(GetProfileVM updateVM);
    Task<GetAccountVM> Get(Guid guid);
}
