using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Assignment;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ClientSide.Repositories;

public class AssignmentRepository : GeneralRepository<AssignmentVM>, IAssignmentRepository
{
    public AssignmentRepository(string request = "Assignment/") : base(request)
    {
    }
    public async Task<ResponseHandlers<IEnumerable<AssignmentVM>>> GetFromManager(Guid guid)
    {
        ResponseHandlers<IEnumerable<AssignmentVM>> entityVM = null;
        using (var response = await _httpClient.GetAsync(_request + "Manager/" + guid))
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<IEnumerable<AssignmentVM>>>(apiResponse);
        }
        return entityVM;
    }

    public async Task<ResponseHandlers<CreateAssignmentVM>> AddAssignment(CreateAssignmentVM createAssignmentVM)
    {
        ResponseHandlers<CreateAssignmentVM> entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(createAssignmentVM), Encoding.UTF8, "application/json");
        using (var response = _httpClient.PostAsync(_request, content).Result)
        {
            string responseApi = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<CreateAssignmentVM>>(responseApi);
        }
        return entityVM;
    }

    public async Task<ResponseHandlers<Guid>> DeepDeleteAssignment(Guid guid)
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

    public async Task<ResponseHandlers<UpdateAssignmentVM>> Update(UpdateAssignmentVM updateAssignmentVM)
    {
        ResponseHandlers<UpdateAssignmentVM> entityVM = null;

        var requestPayload = new UpdateAssignmentVM
        {
            Guid = updateAssignmentVM.Guid,
            Title = updateAssignmentVM.Title,
            Description = updateAssignmentVM.Description,
            DueDate = updateAssignmentVM.DueDate,
            Categories  = updateAssignmentVM.Categories,
           
        };

        StringContent content = new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json");

        using (var response = await _httpClient.PutAsync(_request, content))
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(apiResponse); // Add this line for logging
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<UpdateAssignmentVM>>(apiResponse);
        }

        return entityVM;
    }

}
