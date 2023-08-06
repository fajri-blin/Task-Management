using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.DTOs.CategoryDto;

namespace Task_Management.Service;

public class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly BookingDbContext _bookingContext;

    public CategoryService(ICategoryRepository CategoryRepository, BookingDbContext bookingDbContext)
    {
        _categoryRepository = CategoryRepository;
        _bookingContext = bookingDbContext;
    }

    // Basic CRUD ===================================================
    public IEnumerable<CategoryDto>? Get()
    {
        var entities = _categoryRepository.GetAll();
        if (!entities.Any()) return null;
        var listCategory = new List<CategoryDto>();

        foreach (var entity in entities)
        {
            listCategory.Add((CategoryDto)entity);
        }
        return listCategory;
    }
    // End Basic CRUD =========================================

}
