using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.DTOs.AccountRoleDto;
using Task_Management.Model.Data;

namespace Task_Management.Service;

public class AccountRoleService
{
    private readonly IAccountRoleRepository _accountRoleRepository;
    private readonly BookingDbContext _bookingContext;

    public AccountRoleService(IAccountRoleRepository AccountRoleRepository, BookingDbContext bookingDbContext)
    {
        _accountRoleRepository = AccountRoleRepository;
        _bookingContext = bookingDbContext;
    }

    // Basic CRUD ===================================================
    public IEnumerable<AccountRoleDto>? Get()
    {
        var entities = _accountRoleRepository.GetAll();
        if (!entities.Any()) return null;
        var listAccountRole = new List<AccountRoleDto>();

        foreach ( var entity in entities)
        {
            listAccountRole.Add((AccountRoleDto)entity);
        }
        return listAccountRole;
    }

    public AccountRoleDto? Get(Guid guid)
    {
        var entity = _accountRoleRepository.GetByGuid(guid);
        if (entity is null) return null;

        var Dto = (AccountRoleDto)entity;

        return Dto;
    }

    public AccountRoleDto? Create(NewAccountRoleDto AccountRole)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var created = _accountRoleRepository.Create(AccountRole);
            transaction.Commit();
            return (AccountRoleDto) created;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
    }

    public int Update(AccountRoleDto AccountRoledto)
    {

        var getEntity = _accountRoleRepository.GetByGuid(AccountRoledto.Guid);
        if (getEntity is null) return 0;

        AccountRole AccountRole = (AccountRole) AccountRoledto;
        AccountRole.ModifiedAt = DateTime.Now;
        AccountRole.CreatedAt = getEntity.CreatedAt;

        var transaction = _bookingContext.Database.BeginTransaction();        
        try
        {

            _accountRoleRepository.Update(AccountRole);
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
