using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.DTOs.ProgressDto;
using Task_Management.Model.Data;

namespace Task_Management.Service;

public class ProgressService
{
    private readonly IProgressRepository _progressRepository;
    private readonly IAdditionalRepository _additionalRepository;
    private readonly IAccountProgressRepository _accountProgressRepository;

    private readonly BookingDbContext _bookingContext;

    public ProgressService(IProgressRepository ProgressRepository, 
                           IAdditionalRepository additionalRepository,
                           IAccountProgressRepository accountProgressRepository, 
                           BookingDbContext bookingDbContext)
    {
        _progressRepository = ProgressRepository;
        _additionalRepository = additionalRepository;
        _accountProgressRepository = accountProgressRepository;
        _bookingContext = bookingDbContext;
    }

    public int DeleteDeepProgress(Guid guid)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var getProgress = _progressRepository.GetByGuid(guid);
            if (getProgress is null) return -1;

            var getListAdditional = _additionalRepository.GetByProgressForeignKey(getProgress.Guid);
            if (getListAdditional != null)
            {
                foreach (var additional in getListAdditional)
                {
                    _additionalRepository.Delete(additional);
                }
            }

            var getListAccountProgress = _accountProgressRepository.GetByProgressForeignKey(getProgress.Guid);
            if (getListAccountProgress != null)
            {
                foreach (var accountProgress in getListAccountProgress)
                {
                    _accountProgressRepository.Delete(accountProgress);
                }
            }

            _progressRepository.Delete(getProgress);
            transaction.Commit();
            return 1;
        }catch
        {
            transaction.Rollback();
            return 0;
        }
    }

    // Basic CRUD ===================================================
    public IEnumerable<ProgressDto>? Get()
    {
        var entities = _progressRepository.GetAll();
        if (!entities.Any()) return null;
        var listProgress = new List<ProgressDto>();

        foreach (var entity in entities)
        {
            listProgress.Add((ProgressDto)entity);
        }
        return listProgress;
    }

    public ProgressDto? Get(Guid guid)
    {
        var entity = _progressRepository.GetByGuid(guid);
        if (entity is null) return null;

        var Dto = (ProgressDto)entity;

        return Dto;
    }

    public ProgressDto? Create(NewProgressDto Progress)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var created = _progressRepository.Create(Progress);
            transaction.Commit();
            return (ProgressDto)created;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
    }

    public int Update(ProgressDto Progressdto)
    {

        var getEntity = _progressRepository.GetByGuid(Progressdto.Guid);
        if (getEntity is null) return 0;

        Progress Progress = (Progress)Progressdto;
        Progress.ModifiedAt = DateTime.Now;
        Progress.CreatedAt = getEntity.CreatedAt;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {

            _progressRepository.Update(Progress);
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
        var entity = _progressRepository.GetByGuid(guid);
        if (entity == null) return -1;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            _progressRepository.Delete(entity);
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
