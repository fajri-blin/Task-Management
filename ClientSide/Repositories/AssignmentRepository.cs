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
}
