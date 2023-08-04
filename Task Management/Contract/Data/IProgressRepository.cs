using Task_Management.Model.Data;

namespace Task_Management.Contract.Data;

public interface IProgressRepository : IGeneralRepository<Progress>
{
    IEnumerable<Progress> GetByAssignmentForeignKey(Guid guid);
    Progress GetAnyRelatedByGuid(Guid guid);
}
