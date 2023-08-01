using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Dashboard;
using Newtonsoft.Json;

namespace ClientSide.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _request;
        public DashboardRepository(string request = "Dashboard/")
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7113/api/")
            };
            this._request = request;
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
    }
}
