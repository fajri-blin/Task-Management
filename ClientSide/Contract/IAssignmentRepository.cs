using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Assignment;

namespace ClientSide.Contract;

public interface IAssignmentRepository : IGeneralRepository<AssignmentVM>
{
    Task<ResponseHandlers<IEnumerable<AssignmentVM>>> GetFromManager(Guid guid);
    Task<ResponseHandlers<CreateAssignmentVM>> AddAssignment(CreateAssignmentVM createAssignmentVM);
    Task<ResponseHandlers<Guid>> DeepDeleteAssignment(Guid guid);
}
