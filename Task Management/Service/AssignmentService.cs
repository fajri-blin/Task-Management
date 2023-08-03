using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.Dtos.AssignmentDto;
using Task_Management.DTOs.AssignMapDto;
using Task_Management.DTOs.AssignmentDto;
using Task_Management.DTOs.CategoryDto;
using Task_Management.DTOs.ProgressDto;
using Task_Management.Model.Data;
using Task_Management.Utilities.Enum;

namespace Task_Management.Service;

public class AssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IProgressRepository _progressRepository;
    private readonly IAccountProgressRepository _accountProgressRepository;
    private readonly IAdditionalRepository _additionalRepository;
    private readonly IAssignMapRepository _assignMapRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly BookingDbContext _bookingContext;

    public AssignmentService(IAssignmentRepository assignmentRepository, IProgressRepository progressRepository, BookingDbContext bookingDbContext, IAssignMapRepository assignMapRepository, IAccountProgressRepository accountProgressRepository, ICategoryRepository categoryRepository, IAccountRepository accountRepository)
    {
        _assignmentRepository = assignmentRepository;
        _progressRepository = progressRepository;
        _bookingContext = bookingDbContext;
        _assignMapRepository = assignMapRepository;
        _accountProgressRepository = accountProgressRepository;
        _categoryRepository = categoryRepository;
        _accountRepository = accountRepository;
    }



    public int DeleteDeepAssignment(Guid guid)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var getAssignment = _assignmentRepository.GetByGuid(guid);
            if (getAssignment == null) return -1;

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
            _assignmentRepository.Delete(getAssignment);
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
    public IEnumerable<AssignmentByManagerDto>? Get()
    {
        var assignments = _assignmentRepository.GetAll();
        if (assignments is null) return null;
        var assignmentMaps = _assignMapRepository.GetAll();
        var categories = _categoryRepository.GetAll();
        var progresses = _progressRepository.GetAll();
        var accounts = _accountRepository.GetAll();

        var Dto = (
            from assignment in assignments
            join account in accounts on assignment.ManagerGuid equals account.Guid
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
                account.Name,
                progress = CalculatePercentage(progressGroup.Select(p => p.Status).ToList())

            } into g
            select new AssignmentByManagerDto
            {
                Guid = g.Key.Guid,
                Title = g.Key.Title,
                Description = g.Key.Description,
                DueDate = g.Key.DueDate,
                Name = g.Key.Name,
                category = g.Select(c => c.Name).Distinct().ToList(),
                progress = g.Key.progress
            }
            ).ToList();

        return Dto;
    }

    public AssignmentByManagerDto? Get(Guid guid)
    {
        var assignments = _assignmentRepository.GetAll();
        var assignmentMaps = _assignMapRepository.GetAll();
        var categories = _categoryRepository.GetAll();
        var progresses = _progressRepository.GetAll();
        var accounts = _accountRepository.GetAll();

        var Dto = (
            from assignment in assignments
            join account in accounts on assignment.ManagerGuid equals account.Guid
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
                account.Name,
                progress = CalculatePercentage(progressGroup.Select(p => p.Status).ToList())

            } into g
            select new AssignmentByManagerDto
            {
                Guid = g.Key.Guid,
                Title = g.Key.Title,
                Description = g.Key.Description,
                DueDate = g.Key.DueDate,
                Name = g.Key.Name,
                category = g.Select(c => c.Name).Distinct().ToList(),
                progress = g.Key.progress
            }
            ).ToList();
        var data = Dto.FirstOrDefault(d => d.Guid == guid);
        if (data is null) return null;
        return data;
    }

    public IEnumerable<AssignmentByManagerDto>? GetByManager(Guid guid)
    {
        var assignments = _assignmentRepository.GetByManager(guid);
        if (assignments is null) return null;

        var assignmentMaps = _assignMapRepository.GetAll();
        var categories = _categoryRepository.GetAll();
        var progresses = _progressRepository.GetAll();
        var accounts = _accountRepository.GetAll();

        var Dto = (
            from assignment in assignments
            join account in accounts on assignment.ManagerGuid equals account.Guid
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
                account.Name,
                progress = CalculatePercentage(progressGroup.Select(p => p.Status).ToList())

            } into g
            select new AssignmentByManagerDto
            {
                Guid = g.Key.Guid,
                Title = g.Key.Title,
                Description = g.Key.Description,
                DueDate = g.Key.DueDate,
                Name = g.Key.Name,
                category = g.Select(c => c.Name).Distinct().ToList(),
                progress = g.Key.progress
            }
            ).ToList();

        return Dto;
    }

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
            var createdAssignment = _assignmentRepository.Create(Task);
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

    public int Update(UpdateAssignmentDto updateAssignmentDto)
    {

        var getEntity = _assignmentRepository.GetByGuid(updateAssignmentDto.Guid);
        if (getEntity is null) return 0;

        var assignmentManagerGuid = _assignmentRepository.GetByGuid(updateAssignmentDto.Guid);

        Assignment assignment = (Assignment)updateAssignmentDto;
        assignment.ModifiedAt = DateTime.Now;
        assignment.CreatedAt = getEntity.CreatedAt;
        assignment.ManagerGuid = assignmentManagerGuid.ManagerGuid;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var categories = _categoryRepository.GetAll();

            var taskCategories = updateAssignmentDto.Category;

            var assignmentMaps = _assignMapRepository.GetAll().Where(a => a.AssignmentGuid == updateAssignmentDto.Guid);

            foreach (var assignmentMap in assignmentMaps)
            {
                _assignMapRepository.Delete(assignmentMap);
            }

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
            var updatedAssignment = _assignmentRepository.Update(assignment);
            foreach (var categoryName in taskCategories)
            {
                // Cari ID kategori berdasarkan nama kategori
                var categoryId = categories.FirstOrDefault(c => c.Name == categoryName)?.Guid;

                // Buat entri baru di tabel assignmentmap untuk menghubungkan assignment dengan kategori
                var CreateassignmentMap = new NewAssignMapDto
                {
                    AssignmentGuid = updateAssignmentDto.Guid,
                    CategoryGuid = categoryId ?? _categoryRepository.GetByName(categoryName).Guid
                };
                _assignMapRepository.Create(CreateassignmentMap);
            }

            _assignmentRepository.Update(assignment);
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
        var entity = _assignmentRepository.GetByGuid(guid);
        if (entity == null) return -1;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            _assignmentRepository.Delete(entity);
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
    //AssignmentRepository.cs
    public IEnumerable<GetForStaffDto> GetForStaff(Guid accountGuid)
    {
        var getListAccountProgress = _accountProgressRepository.GetByAccountGuid(accountGuid);
        if (getListAccountProgress is null)
        {
            return null;
        }

        var staffViewDataList = new List<GetForStaffDto>();

        foreach (var accountProgress in getListAccountProgress)
        {
            var progress = _progressRepository.GetByGuid((Guid)accountProgress.ProgressGuid);
            var assignment = _assignmentRepository.GetByGuid(progress.AssignmentGuid.Value);
            var manager = _accountRepository.GetByGuid((Guid)assignment.ManagerGuid);

            var getForStaffDto = new GetForStaffDto
            {
                AssignmentName = assignment.Title,
                ManagerName = manager.Name,
                ListProgress = new List<GetProgressDto>()
            };

            // Here, progress is a single object of type 'Progress', not a collection
            var getProgressDto = new GetProgressDto
            {
                Guid = progress.Guid,
                AssignmentGuid = progress.AssignmentGuid,
                Description = progress.Description,
            };

            getForStaffDto.ListProgress.Add(getProgressDto);

            staffViewDataList.Add(getForStaffDto);
        }

        return staffViewDataList;
    }

}
