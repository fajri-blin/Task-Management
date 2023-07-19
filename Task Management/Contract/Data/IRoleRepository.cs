using Task_Management.Model.Data;

namespace Task_Management.Contract.Data;

public interface IRoleRepository : IGeneralRepository<Role>
{
    Role? GetByName(string name);
}
