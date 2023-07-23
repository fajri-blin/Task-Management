using Task_Management.Model.Data;

namespace Task_Management.Contract.Data;

public interface IAdditionalRepository : IGeneralRepository<Additional>
{
    IEnumerable<Additional> GetByProgressForeignKey(Guid guid);
}
