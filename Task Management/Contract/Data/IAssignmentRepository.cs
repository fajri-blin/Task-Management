using Task_Management.Model.Data;

namespace Task_Management.Contract.Data;

public interface IAssignmentRepository : IGeneralRepository<Assignment>
{
    public IEnumerable<Assignment>? GetByManager(Guid managerId);
}
