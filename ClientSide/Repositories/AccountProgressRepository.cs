﻿using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.AccountProgress;
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
                // Attempt to parse the Data field as a Guid
                if (Guid.TryParse(responseObject.Data.ToString(), out Guid parsedGuid))
                {
                    // If the parsing is successful, assign the parsed Guid to the Data field
                    entityVM = new ResponseHandlers<Guid>
                    {
                        Code = responseObject.Code,
                        Message = responseObject.Message,
                        Data = parsedGuid
                    };
                }
                else
                {
                    // If the parsing fails, handle it accordingly (e.g., log the error, set a default value, etc.)
                    entityVM = new ResponseHandlers<Guid>
                    {
                        Code = responseObject.Code,
                        Message = responseObject.Message,
                        Data = Guid.Empty // Set a default Guid value or handle it as needed.
                    };
                }
            }
        }

        return entityVM;
    }



}
