using Task_Management.Model.Data;
using Task_Management.Utilities.Enum;

namespace Task_Management.Contract.Data;

public interface IRoleRepository : IGeneralRepository<Role>
{
    Role? GetByName(string name);
}
