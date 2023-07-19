using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.DTOs.RoleDto;
using Task_Management.Model.Data;

namespace Task_Management.Service;

public class RoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly BookingDbContext _bookingContext;

    public RoleService(IRoleRepository RoleRepository, BookingDbContext bookingDbContext)
    {
        _roleRepository = RoleRepository;
        _bookingContext = bookingDbContext;
    }

    // Basic CRUD ===================================================
    public IEnumerable<RoleDto>? Get()
    {
        var entities = _roleRepository.GetAll();
        if (!entities.Any()) return null;
        var listRole = new List<RoleDto>();

        foreach ( var entity in entities)
        {
            listRole.Add((RoleDto)entity);
        }
        return listRole;
    }

    public RoleDto? Get(Guid guid)
    {
        var entity = _roleRepository.GetByGuid(guid);
        if (entity is null) return null;

        var Dto = (RoleDto)entity;

        return Dto;
    }

    public RoleDto? Create(NewRoleDto Role)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var created = _roleRepository.Create(Role);
            transaction.Commit();
            return (RoleDto) created;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
    }

    public int Update(RoleDto Roledto)
    {

        var getEntity = _roleRepository.GetByGuid(Roledto.Guid);
        if (getEntity is null) return 0;

        Role Role = (Role) Roledto;
        Role.ModifiedAt = DateTime.Now;
        Role.CreatedAt = getEntity.CreatedAt;

        var transaction = _bookingContext.Database.BeginTransaction();        
        try
        {

            _roleRepository.Update(Role);
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
        var entity = _roleRepository.GetByGuid(guid);
        if(entity == null) return -1;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            _roleRepository.Delete(entity);
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
