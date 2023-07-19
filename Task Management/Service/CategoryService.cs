using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.DTOs.CategoryDto;
using Task_Management.Model.Data;

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

        foreach ( var entity in entities)
        {
            listCategory.Add((CategoryDto)entity);
        }
        return listCategory;
    }

    public CategoryDto? Get(Guid guid)
    {
        var entity = _categoryRepository.GetByGuid(guid);
        if (entity is null) return null;

        var Dto = (CategoryDto)entity;

        return Dto;
    }

    public CategoryDto? Create(NewCategoryDto Category)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var created = _categoryRepository.Create(Category);
            transaction.Commit();
            return (CategoryDto) created;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
    }

    public int Update(CategoryDto Categorydto)
    {

        var getEntity = _categoryRepository.GetByGuid(Categorydto.Guid);
        if (getEntity is null) return 0;

        Category Category = (Category) Categorydto;
        Category.ModifiedAt = DateTime.Now;
        Category.CreatedAt = getEntity.CreatedAt;

        var transaction = _bookingContext.Database.BeginTransaction();        
        try
        {

            _categoryRepository.Update(Category);
            transaction.Commit();
            return 1;
        }
        catch
        {
            transaction.Rollback();
            return 0;
        }
    }

    public int Delete(Guid guid)
    {
        var entity = _categoryRepository.GetByGuid(guid);
        if(entity == null) return -1;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            _categoryRepository.Delete(entity);
            transaction.Commit();
            return 1;
        }
        catch
        {
            transaction.Rollback();
            return 0;
        }
    }
    // End Basic CRUD =========================================

}
