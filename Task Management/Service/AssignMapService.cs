using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.Dtos.AssignMapDto;
using Task_Management.DTOs.AssignMapDto;
using Task_Management.Model.Data;

namespace Task_Management.Service;

public class AssignMapService
{
    private readonly IAssignMapRepository _assignMapRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly BookingDbContext _bookingContext;

    public AssignMapService(IAssignMapRepository TaskCategoryMappingRepository, BookingDbContext bookingDbContext, ICategoryRepository categoryRepository, IAssignmentRepository assignmentRepository)
    {
        _assignMapRepository = TaskCategoryMappingRepository;
        _bookingContext = bookingDbContext;
        _categoryRepository = categoryRepository;
        _assignmentRepository = assignmentRepository;
    }

    public CountTop3CategoryDto? CountCategory(Guid guid)
    {
        var assignments = _assignmentRepository.GetByManager(guid);
        var assignMaps = _assignMapRepository.GetAll();
        var categories = _categoryRepository.GetAll();

        var entity = (
            from assignment in assignments
            join assignMap in assignMaps on assignment.Guid equals assignMap.AssignmentGuid
            join category in categories on assignMap.CategoryGuid equals category.Guid
            group category by category.Name into g
            select new
            {
                category = g.Key,
                count = g.Count()
            }).OrderByDescending(c => c.count).ToList();

        var totalOtherCount = entity.Skip(3).Sum(c => c.count);
        var top3Categories = entity.Take(3).ToList();

        var dto = new CountTop3CategoryDto
        {
            CategoryName = top3Categories.Select(c => c.category).ToList(),
            Count = top3Categories.Select(c => c.count).ToList(),
        };

        dto.CategoryName.Add("Other");
        dto.Count.Add(totalOtherCount);

        return dto;
    }

    // Basic CRUD ===================================================
    public IEnumerable<AssignMapDto>? Get()
    {
        var entities = _assignMapRepository.GetAll();
        if (!entities.Any()) return null;
        var listTaskCategoryMapping = new List<AssignMapDto>();

        foreach (var entity in entities)
        {
            listTaskCategoryMapping.Add((AssignMapDto)entity);
        }
        return listTaskCategoryMapping;
    }

    public AssignMapDto? Get(Guid guid)
    {
        var entity = _assignMapRepository.GetByGuid(guid);
        if (entity is null) return null;

        var Dto = (AssignMapDto)entity;

        return Dto;
    }

    public AssignMapDto? Create(NewAssignMapDto TaskCategoryMapping)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var created = _assignMapRepository.Create(TaskCategoryMapping);
            transaction.Commit();
            return (AssignMapDto)created;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
    }

    public int Update(AssignMapDto TaskCategoryMappingdto)
    {

        var getEntity = _assignMapRepository.GetByGuid(TaskCategoryMappingdto.Guid);
        if (getEntity is null) return 0;

        AssignMap TaskCategoryMapping = (AssignMap)TaskCategoryMappingdto;
        TaskCategoryMapping.ModifiedAt = DateTime.Now;
        TaskCategoryMapping.CreatedAt = getEntity.CreatedAt;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {

            _assignMapRepository.Update(TaskCategoryMapping);
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
        var entity = _assignMapRepository.GetByGuid(guid);
        if (entity == null) return -1;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            _assignMapRepository.Delete(entity);
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
