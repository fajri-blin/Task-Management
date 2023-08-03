using ClientSide.Contract;
using ClientSide.ViewModels.Progress;
using ClientSide.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Net;
using ClientSide.ViewModels.Account;
using ClientSide.ViewModels.Assignment;


namespace ClientSide.Repositories
{
    public class ProgressRepository : GeneralRepository<ProgressVM>, IProgressRepository
    {
        public ProgressRepository(string request = "Progress/") : base(request)
        {
        }
        public async Task<ResponseHandlers<IEnumerable<ProgressVM>>> GetAllProgress()
        {
            try
            {
                var response = await _httpClient.GetAsync(_request); // Make sure to await the API response
                string apiResponse = await response.Content.ReadAsStringAsync();
                var entityVM = JsonConvert.DeserializeObject<ResponseHandlers<IEnumerable<ProgressVM>>>(apiResponse);
                return entityVM;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during data retrieval
                return new ResponseHandlers<IEnumerable<ProgressVM>>
                {
                    Code = 500,
                    Status = "Error",
                    Message = ex.Message
                };
            }
        }

        /* public async Task<ResponseHandlers<IEnumerable<ProgressVM>>> GetAllProgress()
         {
             return await Get();
         }*/

        public async Task<ResponseHandlers<CreateProgressVM>> CreateProgress(CreateProgressVM createProgress)
        {
            ResponseHandlers<CreateProgressVM> entityVM = null;

            var request = new CreateProgressVM
            {
                AssignmentGuid = createProgress.AssignmentGuid,
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            using (var response = await _httpClient.PostAsync(_request, content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(apiResponse); 
                entityVM = JsonConvert.DeserializeObject<ResponseHandlers<CreateProgressVM>>(apiResponse);
            }

            return entityVM;
        }


        /*public async Task<ResponseHandlers<ProgressVM>> CreateProgress(ProgressVM progress)
        {
            return await Post(progress);

            ResponseHandlers<CreateProgressVM> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(createProgress), Encoding.UTF8, "application/json");
            using (var response = _httpClient.PostAsync(_request, content).Result)
            {
                string responseApi = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandlers<CreateProgressVM>>(responseApi);
            }
            return entityVM;
        }*/

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
       /* public async Task<ResponseHandlers<IEnumerable<ProgressVM>>> GetAllProgressByAssignmentGuid(Guid assignmentGuid)
        {
            try
            {
                // Get all progress items from the repository
                var allProgress = await Get();

                // Filter progress items based on the provided assignmentGuid
                var filteredProgress = allProgress.Data.Where(p => p.AssignmentGuid == assignmentGuid).ToList();

                // Create a new ResponseHandlers instance with the filtered progress items
                var response = new ResponseHandlers<IEnumerable<ProgressVM>>
                {
                    Code = 200, // Set the appropriate status code for success (200 for OK)
                    Status = "Success",
                    Data = filteredProgress
                };

                // Return the ResponseHandlers object
                return response;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during data retrieval
                var response = new ResponseHandlers<IEnumerable<ProgressVM>>
                {
                    Code = 500,
                    Status = "Error",
                    Message = ex.Message
                };

                return response;
            }
        }*/

        /*public async Task<ResponseHandlers<ProgressVM>> UpdateProgress(UpdateProgressVM updateProgress)
       {
           return await Put(updateProgress);
       }*/

        /*public async Task<ResponseHandlers<Guid>> DeleteProgress(Guid guid)
        {
            return await Delete(guid);
           
        }*/
        /* public async Task<ProgressVM> GetProgressByAssignmentGuid(Guid guid)
        {
            using (var response = await _httpClient.GetAsync($"{_request}{guid}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ProgressVM progress = JsonConvert.DeserializeObject<ProgressVM>(apiResponse);
                    return progress;
                }
                else
                {
                    return null;
                }
            }
        }*/
    }
}
