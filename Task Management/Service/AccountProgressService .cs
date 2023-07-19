using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.DTOs.AccountProgressDto;
using Task_Management.Model.Data;

namespace Task_Management.Service;

public class AccountProgressService
{
    private readonly IAccountProgressRepository _accountRoleRepository;
    private readonly BookingDbContext _bookingContext;

    public AccountProgressService(IAccountProgressRepository AccountProgressRepository, BookingDbContext bookingDbContext)
    {
        _accountRoleRepository = AccountProgressRepository;
        _bookingContext = bookingDbContext;
    }

    // Basic CRUD ===================================================
    public IEnumerable<AccountProgressDto>? Get()
    {
        var entities = _accountRoleRepository.GetAll();
        if (!entities.Any()) return null;
        var listAccountProgress = new List<AccountProgressDto>();

        foreach ( var entity in entities)
        {
            listAccountProgress.Add((AccountProgressDto)entity);
        }
        return listAccountProgress;
    }

    public AccountProgressDto? Get(Guid guid)
    {
        var entity = _accountRoleRepository.GetByGuid(guid);
        if (entity is null) return null;

        var Dto = (AccountProgressDto)entity;

        return Dto;
    }

    public AccountProgressDto? Create(NewAccountProgressDto AccountProgress)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var created = _accountRoleRepository.Create(AccountProgress);
            transaction.Commit();
            return (AccountProgressDto) created;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
    }

    public int Update(AccountProgressDto AccountProgressdto)
    {

        var getEntity = _accountRoleRepository.GetByGuid(AccountProgressdto.Guid);
        if (getEntity is null) return 0;

        AccountProgress AccountProgress = (AccountProgress) AccountProgressdto;
        AccountProgress.ModifiedAt = DateTime.Now;
        AccountProgress.CreatedAt = getEntity.CreatedAt;

        var transaction = _bookingContext.Database.BeginTransaction();        
        try
        {

            _accountRoleRepository.Update(AccountProgress);
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
        var entity = _accountRoleRepository.GetByGuid(guid);
        if(entity == null) return -1;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            _accountRoleRepository.Delete(entity);
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
