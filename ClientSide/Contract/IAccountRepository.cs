using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Account;
using ClientSide.ViewModels.Profile;

namespace ClientSide.Contract;

public interface IAccountRepository : IGeneralRepository<AccountVM>
{
    Task<ResponseHandlers<string>> Login(SignInVM signInDto);
    Task<ResponseHandlers<RegisterVM>> Register(RegisterVM registerDto);
    Task<ResponseHandlers<ForgotPasswordVM>> ForgotPassword(ForgotPasswordVM forgotPasswordVM);
    Task<ResponseHandlers<UpdateVM>> Update(UpdateVM updateVM);
    Task<ResponseHandlers<GetProfileVM>> UpdateProfile(GetProfileVM updateVM);
    Task<UpdateVM> Get(Guid guid);
}
