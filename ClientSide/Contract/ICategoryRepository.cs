using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Category;

namespace ClientSide.Contract;

public interface ICategoryRepository : IGeneralRepository<CategoryVM>
{
    Task<ResponseHandlers<IEnumerable<string>>> GetAllCategories();
}
