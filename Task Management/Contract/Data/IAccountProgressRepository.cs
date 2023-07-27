using Task_Management.Model.Data;

namespace Task_Management.Contract.Data;

public interface IAccountProgressRepository : IGeneralRepository<AccountProgress>
{
    IEnumerable<AccountProgress> GetByProgressForeignKey(Guid guid);

    IEnumerable<AccountProgress>? GetByAccountGuid(Guid accountGuid);
}
