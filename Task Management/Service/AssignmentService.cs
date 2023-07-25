using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.Dtos.AssignmentDto;
using Task_Management.DTOs.AssignMapDto;
using Task_Management.DTOs.AssignmentDto;
using Task_Management.DTOs.CategoryDto;
using Task_Management.Model.Data;
using Task_Management.Utilities.Enum;

namespace Task_Management.Service;

public class AssignmentService
{
    private readonly IAssignmentRepository _assignemtnRepository;
    private readonly IProgressRepository _progressRepository;
    private readonly IAccountProgressRepository _accountProgressRepository;
    private readonly IAdditionalRepository _additionalRepository;
    private readonly IAssignMapRepository _assignMapRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly BookingDbContext _bookingContext;

    public AssignmentService(IAssignmentRepository TaskRepository, IProgressRepository progressRepository, BookingDbContext bookingDbContext, IAssignMapRepository assignMapRepository, IAccountProgressRepository accountProgressRepository, ICategoryRepository categoryRepository)
    {
        _assignemtnRepository = TaskRepository;
        _progressRepository = progressRepository;
        _bookingContext = bookingDbContext;
        _assignMapRepository = assignMapRepository;
        _accountProgressRepository = accountProgressRepository;
        _categoryRepository = categoryRepository;
    }

    public int DeleteDeepAssignment(Guid guid)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var getAssignment = _assignemtnRepository.GetByGuid(guid);
            if (getAssignment != null) return -1;

            var getListProgress = _progressRepository.GetByAssignmentForeignKey(getAssignment.Guid);
            if (getListProgress != null)
            {
                foreach (var progress in getListProgress)
                {
                    var getListAccountProgress = _accountProgressRepository.GetByProgressForeignKey(progress.Guid);
                    if (getListAccountProgress != null)
                    {
                        foreach (var accountProgress in getListAccountProgress)
                        {
                            _accountProgressRepository.Delete(accountProgress);
                        }
                    }
                    var getListAdditional = _additionalRepository.GetByProgressForeignKey(progress.Guid);
                    if (getListAdditional != null)
                    {
                        foreach (var additional in getListAdditional)
                        {
                            _additionalRepository.Delete(additional);
                        }
                    }
                    _progressRepository.Delete(progress);
                }
            }
            _assignemtnRepository.Delete(getAssignment);
            transaction.Commit();
            return 1;
        }
        catch
        {
            transaction.Rollback();
            return 0;
        }
    }

    public double CalculatePercentage(List<StatusEnum> progresses)
    {
        int totalAssignments = progresses.Count;
        int doneAssignments = progresses.Count(progress => progress == StatusEnum.Done);

        if (totalAssignments == 0)
        {
            // To avoid division by zero, handle the case when there are no assignments
            return 0;
        }

        double percentage = (double)doneAssignments / totalAssignments * 100;
        return percentage;
    }

    // Basic CRUD ===================================================
    public IEnumerable<AssignmentDto>? Get()
    {
        var entities = _assignemtnRepository.GetAll();
        if (!entities.Any()) return null;
        var listTask = new List<AssignmentDto>();

        foreach (var entity in entities)
        {
            listTask.Add((AssignmentDto)entity);
        }
        return listTask;
    }

    public AssignmentDto? Get(Guid guid)
    {
        var entity = _assignemtnRepository.GetByGuid(guid);
        if (entity is null) return null;

        var Dto = (AssignmentDto)entity;

        return Dto;
    }

    public IEnumerable<AssignmentByManagerDto>? GetByManager(Guid guid)
    {
        var assignments = _assignemtnRepository.GetByManager(guid);
        if (assignments is null) return null;

        var assignmentMaps = _assignMapRepository.GetAll();
        var categories = _categoryRepository.GetAll();
        var progresses = _progressRepository.GetAll();

        /*var assigmentProgress = _progressRepository.GetByAssignmentForeignKey()*/

        var Dto = (
            from assignment in assignments
            join assignmentMap in assignmentMaps on assignment.Guid equals assignmentMap.AssignmentGuid
            join category in categories on assignmentMap.CategoryGuid equals category.Guid
            join progress in progresses on assignment.Guid equals progress.AssignmentGuid into progressGroup
            from progressItem in progressGroup.DefaultIfEmpty()
            group category by new
            {
                assignment.Guid,
                assignment.Title,
                assignment.Description,
                assignment.DueDate,
                assignment.ManagerGuid,
                progress = CalculatePercentage(progressGroup.Select(p => p.Status).ToList())

            } into g
            select new AssignmentByManagerDto
            {
                Guid = g.Key.Guid,
                Title = g.Key.Title,
                Description = g.Key.Description,
                DueDate = g.Key.DueDate,
                ManagerGuid = g.Key.ManagerGuid,
                category = g.Select(c => c.Name).Distinct().ToList(),
                progress = g.Key.progress
            }
            ).ToList();

        return Dto;
    }

    /*public AssignmentDto? Create(NewAssignmentDto Task)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var created = _assignemtnRepository.Create(Task);
            transaction.Commit();
            return (AssignmentDto)created;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
    }*/

    public AssignmentDto? Create(NewAssignmentDto Task)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var categories = _categoryRepository.GetAll();

            var taskCategories = Task.Category;

            // Mengecek apakah ada kategori yang sama di antara kedua koleksi
            var commonCategories = categories.Select(c => c.Name).Intersect(taskCategories).ToList();

            // Jika tidak ada kategori yang sama, buat kategori baru di tabel category
            var newCategories = taskCategories.Except(commonCategories).ToList();
            foreach (var categoryName in newCategories)
            {
                // Periksa apakah kategori sudah ada di dalam tabel
                var existingCategory = categories.FirstOrDefault(c => c.Name == categoryName);
                if (existingCategory == null)
                {
                    var newCategory = new NewCategoryDto
                    {
                        Name = categoryName
                    };
                    _categoryRepository.Create(newCategory);
                }
            }

            // Jika ada kategori yang sama atau baru saja dibuat, lanjutkan proses penyimpanan data
            var createdAssignment = _assignemtnRepository.Create(Task);
            foreach (var categoryName in taskCategories)
            {
                // Cari ID kategori berdasarkan nama kategori
                var categoryId = categories.FirstOrDefault(c => c.Name == categoryName)?.Guid;

                // Buat entri baru di tabel assignmentmap untuk menghubungkan assignment dengan kategori
                var assignmentMap = new NewAssignMapDto
                {
                    AssignmentGuid = createdAssignment.Guid,
                    CategoryGuid = categoryId ?? _categoryRepository.GetByName(categoryName).Guid
                };
                _assignMapRepository.Create(assignmentMap);
            }

            transaction.Commit();
            return (AssignmentDto)createdAssignment;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
    }

    public int Update(AssignmentDto assignmentDto)
    {

        var getEntity = _assignemtnRepository.GetByGuid(assignmentDto.Guid);
        if (getEntity is null) return 0;

        Assignment assignment = (Assignment)assignmentDto;
        assignment.ModifiedAt = DateTime.Now;
        assignment.CreatedAt = getEntity.CreatedAt;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {

            _assignemtnRepository.Update(assignment);
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
        var entity = _assignemtnRepository.GetByGuid(guid);
        if (entity == null) return -1;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            _assignemtnRepository.Delete(entity);
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
