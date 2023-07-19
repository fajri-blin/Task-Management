using Task_Management.Model.Data;

namespace Task_Management.Contract.Data;

public interface IAccountRoleRepository : IGeneralRepository<AccountRole>
{
    IEnumerable<AccountRole> GetAccountRolesByAccountGuid(Guid guid);
}
