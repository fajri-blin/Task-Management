using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Account;
using Newtonsoft.Json;
using System.Text;

namespace ClientSide.Repositories;

public class AccountRepository : GeneralRepository<AccountDto>, IAccountRepository
{

    public AccountRepository(string request = "Account/") : base(request) { }

    public async Task<ResponseHandlers<string>> Login(SignInDto signInDto)
    {
        ResponseHandlers<string> entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(signInDto), Encoding.UTF8, "application/json");
        using (var response = _httpClient.PostAsync(_request + "Login", content).Result)
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<string>>(apiResponse);
        }
        return entityVM;
    }

    public async Task<ResponseHandlers<RegisterDto>> Register(RegisterDto registerDto)
    {
        ResponseHandlers<RegisterDto> entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");
        using (var response = _httpClient.PostAsync(_request + "Register", content).Result)
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<RegisterDto>>(apiResponse);
        }
        return entityVM;
    }
}
