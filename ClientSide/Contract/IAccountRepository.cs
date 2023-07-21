using ClientSide.ViewModels.Account;
using Task_Management.DTOs.AccountDto;
using Task_Management.Utilities.Handler;

namespace ClientSide.Contract;

public interface IAccountRepository : IGeneralRepository<AccountDto>
{
    Task<ResponseHandlers<string>> Login(SignInDto signInDto);
    Task<ResponseHandlers<RegisterDto>> Register(RegisterDto registerDto);
}
