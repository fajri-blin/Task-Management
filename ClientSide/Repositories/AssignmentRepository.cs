using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Assignment;
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
}
