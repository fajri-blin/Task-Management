using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Account;

namespace ClientSide.Contract;

public interface IAccountRepository : IGeneralRepository<AccountDto>
{
    Task<ResponseHandlers<string>> Login(SignInDto signInDto);
    Task<ResponseHandlers<RegisterDto>> Register(RegisterDto registerDto);
}
