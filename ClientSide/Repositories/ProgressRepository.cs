using ClientSide.Contract;
using ClientSide.ViewModels.Progress;
using ClientSide.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ClientSide.Repositories
{
    public class ProgressRepository : GeneralRepository<ProgressVM>, IProgressRepository
    {
        public ProgressRepository(string request = "Progress/") : base(request)
        {
        }
        public async Task<ResponseHandlers<ProgressVM>> GetProgressById(Guid guid)
        {
            return await Get(guid);
        }

        public async Task<ResponseHandlers<IEnumerable<ProgressVM>>> GetAllProgress()
        {
            return await Get();
        }

        public async Task<ResponseHandlers<ProgressVM>> CreateProgress(ProgressVM progress)
        {
            return await Post(progress);
        }

        public async Task<ResponseHandlers<UpdateProgressVM>> UpdateProgress(UpdateProgressVM updateProgress)
        {
            ResponseHandlers<UpdateProgressVM> entityVM = null;

            var requestPayload = new UpdateProgressVM
            {
                Guid = updateProgress.Guid,
                AssignmentGuid = updateProgress.AssignmentGuid,
                Description = updateProgress.Description,
                Status = updateProgress.Status,
                Additional = updateProgress.Additional,
                MessageManager = updateProgress.MessageManager,
                DueDate = updateProgress.DueDate,
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json");

            using (var response = await _httpClient.PutAsync(_request, content))
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
        /*public async Task<ResponseHandlers<ProgressVM>> UpdateProgress(UpdateProgressVM updateProgress)
       {
           return await Put(updateProgress);
       }*/

        /*public async Task<ResponseHandlers<Guid>> DeleteProgress(Guid guid)
        {
            return await Delete(guid);
           
        }*/
    }
}
