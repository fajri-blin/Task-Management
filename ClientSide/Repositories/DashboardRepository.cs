using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Dashboard;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ClientSide.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        protected readonly string _request;
        protected readonly HttpClient _httpClient;
        protected readonly IHttpContextAccessor _contextAccessor;
        public DashboardRepository(string request = "Dashboard/")
        {
            _contextAccessor = new HttpContextAccessor();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7113/api/")
            };
            this._request = request;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _contextAccessor.HttpContext?.Session.GetString("JWToken"));
        }

        public async Task<ResponseHandlers<DashboardMonthMangerVM>> CountMonth(Guid guid)
        {
            ResponseHandlers<DashboardMonthMangerVM> entityVM = null;
            using (var response = await _httpClient.GetAsync(_request + "Manager/CountMonth/" + guid))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandlers<DashboardMonthMangerVM>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<ResponseHandlers<DashboardCategoryManagerVM>> CountCategory(Guid guid)
        {
            ResponseHandlers<DashboardCategoryManagerVM> entityVM = null;
            using (var response = await _httpClient.GetAsync(_request + "Manager/CountCategory/" + guid))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandlers<DashboardCategoryManagerVM>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<ResponseHandlers<DashboardRoleAdminVM>> CountRole()
        {
            ResponseHandlers<DashboardRoleAdminVM> entityVM = null;
            using (var response = await _httpClient.GetAsync(_request + "Admin/CountAccount"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandlers<DashboardRoleAdminVM>>(apiResponse);
            }
            return entityVM;
        }
    }
}
