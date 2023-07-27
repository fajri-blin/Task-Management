using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Account;
using Newtonsoft.Json;
using System.Text;

namespace ClientSide.Repositories;

public class AccountRepository : GeneralRepository<AccountVM>, IAccountRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _request;

    public AccountRepository(string request = "Account/") : base(request)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7113/api/")
        };
        this._request = request;
    }
    public async Task<ResponseHandlers<UpdateVM>> Update(UpdateVM updateVM)
    {
        ResponseHandlers<UpdateVM> entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(updateVM), Encoding.UTF8, "application/json");
        using (var response = _httpClient.PutAsync(_request + "Update", content).Result)
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<UpdateVM>>(apiResponse);
        }
        return entityVM;
    }
    public async Task<UpdateVM> Get(Guid guid)
    {
        using (var response = await _httpClient.GetAsync($"{_request}{guid}"))
        {
            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                UpdateVM updateVM = JsonConvert.DeserializeObject<UpdateVM>(apiResponse);
                return updateVM;
            }
            else
            {
                return null;
            }
        }
    }

    public async Task<ResponseHandlers<ForgotPasswordVM>> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
    {        
        ResponseHandlers<ForgotPasswordVM> entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(forgotPasswordVM), Encoding.UTF8, "application/json");
        using (var response = _httpClient.PostAsync(_request + "ForgotPassword", content).Result)
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<ForgotPasswordVM>>(apiResponse);
        }
        return entityVM;
    }

    public async Task<ResponseHandlers<string>> Login(SignInVM signInDto)
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

    public async Task<ResponseHandlers<RegisterVM>> Register(RegisterVM registerDto)
    {
        ResponseHandlers<RegisterVM> entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");
        using (var response = _httpClient.PostAsync(_request + "Register", content).Result)
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<RegisterVM>>(apiResponse);
        }
        return entityVM;
    }
}
