using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Account;

namespace ClientSide.Contract;

public interface IAccountRepository : IGeneralRepository<AccountVM>
{
    Task<ResponseHandlers<string>> Login(SignInVM signInDto);
    Task<ResponseHandlers<RegisterVM>> Register(RegisterVM registerDto);
    Task<ResponseHandlers<ForgotPasswordVM>> ForgotPassword(ForgotPasswordVM forgotPasswordVM);
}
