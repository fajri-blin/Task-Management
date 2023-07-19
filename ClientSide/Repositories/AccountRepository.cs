using ClientSide.Contract;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;
using Task_Management.DTOs.AccountDto;
using Task_Management.Utilities.Handler;

namespace ClientSide.Repositories;

public class AccountRepository : GeneralRepository<AccountDto>,IAccountRepository
{

    public AccountRepository(string request = "Account/") : base(request) { }

    public async Task<ResponseHandlers<string>> Login(LoginDto loginDto)
    {
        ResponseHandlers<string> entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
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
