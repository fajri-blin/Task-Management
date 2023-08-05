using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.AccountProgress;
using ClientSide.ViewModels.Assignment;
using ClientSide.ViewModels.Progress;
using Newtonsoft.Json;
using System.Text;

namespace ClientSide.Repositories;

public class AccountProgressRepository : GeneralRepository<AccountProgressVM>, IAccountProgressRepository
{
    public AccountProgressRepository(string request = "AccountProgress/") : base(request) { }

    public async Task<ResponseHandlers<IEnumerable<AccountProgressVM>>> GetByProgress(Guid guid)
    {
        ResponseHandlers<IEnumerable<AccountProgressVM>> entityVM = null;
        using (var response = await _httpClient.GetAsync(_request + "GetByProgress/" + guid))
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<IEnumerable<AccountProgressVM>>>(apiResponse);
        }
        return entityVM;
    }

    public async Task<ResponseHandlers<IEnumerable<AccountProgressVM>>> GetByAccount(Guid guid)
    {
        ResponseHandlers<IEnumerable<AccountProgressVM>> entityVM = null;
        using (var response = await _httpClient.GetAsync(_request + "GetByAccountGuid/" + guid))
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<IEnumerable<AccountProgressVM>>>(apiResponse);
        }
        return entityVM;
    }

    public async Task<ResponseHandlers<AssignStaff>> AddAccountProgress(AssignStaff accountProgressVM)
    {
        ResponseHandlers<AssignStaff> entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(accountProgressVM), Encoding.UTF8, "application/json");
        using (var response = _httpClient.PostAsync(_request, content).Result)
        {
            string responseApi = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<AssignStaff>>(responseApi);
        }
        return entityVM;
    }

    public async Task<ResponseHandlers<Guid>> DeleteAccountProgress(Guid guid)
    {
        ResponseHandlers<Guid> entityVM = null;

        string requestUrl = _request.TrimEnd('/') + "?guid=" + guid;

        using (var response = await _httpClient.DeleteAsync(requestUrl))
        {
            string responseApi = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<ResponseHandlers<object>>(responseApi);

            // Check if the data field is null
            if (responseObject.Data == null)
            {
                entityVM = new ResponseHandlers<Guid>
                {
                    Code = responseObject.Code,
                    Message = responseObject.Message
                };
            }
            else
            {
                // If the data field is not null, deserialize it to Guid
                entityVM = new ResponseHandlers<Guid>
                {
                    Code = responseObject.Code,
                    Message = responseObject.Message,
                    Data = Guid.Parse(responseObject.Data.ToString())
                };
            }
        }

        return entityVM;
    }


}
