using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Category;
using Newtonsoft.Json;

namespace ClientSide.Repositories;

public class CategoryRepository : GeneralRepository<CategoryVM>, ICategoryRepository
{
    public CategoryRepository(string request = "Category/") : base(request)
    {

    }

    public async Task<ResponseHandlers<IEnumerable<string>>> GetAllCategories()
    {
        ResponseHandlers<IEnumerable<string>> entityVM = null;
        using (var response = await _httpClient.GetAsync(_request + "Categories"))
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<IEnumerable<string>>>(apiResponse);
        }
        return entityVM;
    }
}
