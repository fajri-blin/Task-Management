using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Account;
using ClientSide.ViewModels.Profile;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
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

    public async Task<ResponseHandlers<CheckOTPVM>> CheckAccountOTP(CheckOTPVM checkOTPVM)
    {
        ResponseHandlers<CheckOTPVM> entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(checkOTPVM), Encoding.UTF8, "application/json");
        using(var response = _httpClient.PostAsync(_request + "CheckOtp", content).Result)
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<CheckOTPVM>>(apiResponse);
        }
        return entityVM;
    }

    public async Task<ResponseHandlers<ChangePasswordVM>> ChangeAccountPassword(ChangePasswordVM changePasswordVM)
    {
        ResponseHandlers<ChangePasswordVM> entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(changePasswordVM), Encoding.UTF8, "application/json");
        using(var response = _httpClient.PutAsync(_request + "changePassword", content).Result)
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<ChangePasswordVM>>(apiResponse);
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

    public async Task<ResponseHandlers<GetProfileVM>> UpdateProfile([FromForm] GetProfileVM updateVM)
    {
        ResponseHandlers<GetProfileVM> entityVM = null;

        using (var formData = new MultipartFormDataContent())
        {
            // Tambahkan foto profile ke form data
            if (updateVM.ImageProfile != null)
            {
                var streamContent = new StreamContent(updateVM.ImageProfile.OpenReadStream())
                {
                    Headers = { ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "ImageProfile", FileName = updateVM.ImageProfile.FileName } }
                };
                formData.Add(streamContent, "ImageProfile", updateVM.ImageProfile.FileName);
            }

            // Tambahkan data lainnya ke form data
            formData.Add(new StringContent(updateVM.Guid.ToString()), "Guid");
            formData.Add(new StringContent(updateVM.Name), "Name");
            formData.Add(new StringContent(updateVM.Username), "Username");
            formData.Add(new StringContent(updateVM.Email), "Email");
            if (!string.IsNullOrEmpty(updateVM.Password))
            {
                formData.Add(new StringContent(updateVM.Password), "Password");
            }
            if (!string.IsNullOrEmpty(updateVM.ConfirmPassword))
            {
                formData.Add(new StringContent(updateVM.ConfirmPassword), "ConfirmPassword");
            }

            // Tambahkan data lainnya sesuai kebutuhan

            // Kirim request ke API
            using (var response = await _httpClient.PutAsync(_request + "Profile/Update", formData))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandlers<GetProfileVM>>(apiResponse);
            }
        }

        return entityVM;
    }
}
