using Task_Management.Model.Data;

namespace Task_Management.Contract.Data;

public interface ICategoryRepository : IGeneralRepository<Category>
{
    Category? GetByName(string name);
}