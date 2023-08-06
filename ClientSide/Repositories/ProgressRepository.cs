using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Progress;
using Newtonsoft.Json;
using System.Text;



namespace ClientSide.Repositories
{
    public class ProgressRepository : GeneralRepository<ProgressVM>, IProgressRepository
    {
        public ProgressRepository(string request = "Progress/") : base(request)
        {
        }
        public async Task<ResponseHandlers<IEnumerable<ProgressVM>>> GetAllProgress(Guid guid)
        {
            var response = await _httpClient.GetAsync(_request + "GetByAssignmentKey/" + guid);
            string apiResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new ResponseHandlers<IEnumerable<ProgressVM>>
                {
                    Code = (int)response.StatusCode,
                    Status = response.StatusCode.ToString(),
                    Message = apiResponse
                };
            }
            var entityVM = JsonConvert.DeserializeObject<ResponseHandlers<IEnumerable<ProgressVM>>>(apiResponse);
            return entityVM;
        }

        public async Task<ResponseHandlers<CreateProgressVM>> CreateProgress(CreateProgressVM createProgress)
        {
            ResponseHandlers<CreateProgressVM> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(createProgress), Encoding.UTF8, "application/json");
            using (var response = _httpClient.PostAsync(_request, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandlers<CreateProgressVM>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<ResponseHandlers<UpdateProgressVM>> UpdateProgress(UpdateProgressVM updateProgress)
        {
            ResponseHandlers<UpdateProgressVM> entityVM = null;

            var requestPayload = new UpdateProgressVM
            {
                Guid = updateProgress.Guid,
                AssignmentGuid = updateProgress.AssignmentGuid,
                AccountGuid = updateProgress.AccountGuid,
                Description = updateProgress.Description,
                Status = updateProgress.Status,
                Additional = updateProgress.Additional,
                MessageManager = updateProgress.MessageManager,
                DueDate = updateProgress.DueDate,
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json");

            using (var response = await _httpClient.PutAsync(_request + "Status/", content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(apiResponse);
                entityVM = JsonConvert.DeserializeObject<ResponseHandlers<UpdateProgressVM>>(apiResponse);
            }
            return entityVM;
        }
        public async Task<ResponseHandlers<Guid>> DeepDeleteProgress(Guid guid)
        {
            ResponseHandlers<Guid> entityVM = null;

            using (var response = await _httpClient.DeleteAsync(_request + "DeepDelete/" + guid))
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

        public async Task<ResponseHandlers<ProgressVM>> GetProgressById(Guid guid)
        {
            using (var httpResponse = await _httpClient.GetAsync(_request + guid))
            {
                string apiResponse = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseHandlers<ProgressVM>>(apiResponse);
            }
        }
        public async Task<ResponseHandlers<ProgressVM>> UpdateProgress(ProgressVM progress)
        {
            string json = JsonConvert.SerializeObject(progress);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PutAsync(_request, httpContent))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseHandlers<ProgressVM>>(apiResponse);
            }
        }
    }
}
