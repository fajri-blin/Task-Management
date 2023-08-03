using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Account;
using ClientSide.ViewModels.Assignment;
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

    public async Task<ResponseHandlers<GetForStaffVM>> GetAssignmentForStaff(Guid guid)
    {
        ResponseHandlers<GetForStaffVM> entityVM = null;
        using (var response = _httpClient.GetAsync(_request + "GetForStaff" + guid).Result)
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<GetForStaffVM>>(apiResponse);
        }
        return entityVM;
    }


    public async Task<ResponseHandlers<UpdateAccountVM>> Update([FromForm] UpdateAccountVM updateAccountVM)
    {
        ResponseHandlers<UpdateAccountVM> entityVM = null;

        using (var formData = new MultipartFormDataContent())
        {
            // Tambahkan foto profile ke form data
            if (updateAccountVM.ImageProfile != null)
            {
                var streamContent = new StreamContent(updateAccountVM.ImageProfile.OpenReadStream())
                {
                    Headers = { ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "ImageProfile", FileName = updateAccountVM.ImageProfile.FileName } }
                };
                formData.Add(streamContent, "ImageProfile", updateAccountVM.ImageProfile.FileName);
            }

            // Tambahkan data lainnya ke form data
            formData.Add(new StringContent(updateAccountVM.Guid.ToString()), "Guid");
            formData.Add(new StringContent(updateAccountVM.Name), "Name");
            formData.Add(new StringContent(updateAccountVM.Username), "Username");
            formData.Add(new StringContent(updateAccountVM.Email), "Email");
            formData.Add(new StringContent(updateAccountVM.Role.ToString()), "RoleGuid");
            if (!string.IsNullOrEmpty(updateAccountVM.Password))
            {
                formData.Add(new StringContent(updateAccountVM.Password), "Password");
            }
            if (!string.IsNullOrEmpty(updateAccountVM.ConfirmPassword))
            {
                formData.Add(new StringContent(updateAccountVM.ConfirmPassword), "ConfirmPassword");
            }

            // Tambahkan data lainnya sesuai kebutuhan

            // Kirim request ke API
            using (var response = await _httpClient.PutAsync(_request, formData))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandlers<UpdateAccountVM>>(apiResponse);
            }
        }

        return entityVM;
    }
    public async Task<GetAccountVM> Get(Guid guid)
    {
        using (var response = await _httpClient.GetAsync($"{_request}{guid}"))
        {
            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                GetAccountVM updateVM = JsonConvert.DeserializeObject<GetAccountVM>(apiResponse);
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

    public async Task<ResponseHandlers<GetProfileVM>> UpdateProfile([FromForm] GetProfileVM getProfileVM)
    {
        ResponseHandlers<GetProfileVM> entityVM = null;

        using (var formData = new MultipartFormDataContent())
        {
            // Tambahkan foto profile ke form data
            if (getProfileVM.ImageProfile != null)
            {
                var streamContent = new StreamContent(getProfileVM.ImageProfile.OpenReadStream())
                {
                    Headers = { ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "ImageProfile", FileName = getProfileVM.ImageProfile.FileName } }
                };
                formData.Add(streamContent, "ImageProfile", getProfileVM.ImageProfile.FileName);
            }

            // Tambahkan data lainnya ke form data
            formData.Add(new StringContent(getProfileVM.Guid.ToString()), "Guid");
            formData.Add(new StringContent(getProfileVM.Name), "Name");
            formData.Add(new StringContent(getProfileVM.Username), "Username");
            formData.Add(new StringContent(getProfileVM.Email), "Email");
            if (!string.IsNullOrEmpty(getProfileVM.Password))
            {
                formData.Add(new StringContent(getProfileVM.Password), "Password");
            }
            if (!string.IsNullOrEmpty(getProfileVM.ConfirmPassword))
            {
                formData.Add(new StringContent(getProfileVM.ConfirmPassword), "ConfirmPassword");
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
