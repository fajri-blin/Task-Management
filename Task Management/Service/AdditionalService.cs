using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.DTOs.AdditionalDto;
using Task_Management.DTOs.NewAdditionalDto;
using Task_Management.Model.Data;

namespace Task_Management.Service;

public class AdditionalService
{
    private readonly IAdditionalRepository _additionalRepository;
    private readonly BookingDbContext _bookingContext;

    public AdditionalService(IAdditionalRepository AdditionalRepository, BookingDbContext bookingDbContext)
    {
        _additionalRepository = AdditionalRepository;
        _bookingContext = bookingDbContext;
    }

    public IEnumerable<AdditionalDto> GetByProgressGuid(Guid guid)
    {
        var list = _additionalRepository.GetByProgressForeignKey(guid);
        if (list == null) return null;

        var baseList = new List<AdditionalDto>();
        foreach(var item in list)
        {
            baseList.Add((AdditionalDto)item);
        }
        return baseList;
    }

    // Basic CRUD ===================================================
    public IEnumerable<AdditionalDto>? Get()
    {
        var entities = _additionalRepository.GetAll();
        if (!entities.Any()) return null;
        var listAdditional = new List<AdditionalDto>();

        foreach ( var entity in entities)
        {
            listAdditional.Add((AdditionalDto)entity);
        }
        return listAdditional;
    }

    public AdditionalDto? Get(Guid guid)
    {
        var entity = _additionalRepository.GetByGuid(guid);
        if (entity is null) return null;

        var Dto = (AdditionalDto)entity;

        return Dto;
    }

    public AdditionalDto? Create(NewAdditionalDto Additional)
    {
        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            var created = _additionalRepository.Create(Additional);
            transaction.Commit();
            return (AdditionalDto) created;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
    }

    public int Update(AdditionalDto Additionaldto)
    {

        var getEntity = _additionalRepository.GetByGuid(Additionaldto.Guid);
        if (getEntity is null) return 0;

        Additional Additional = (Additional) Additionaldto;
        Additional.ModifiedAt = DateTime.Now;
        Additional.CreatedAt = getEntity.CreatedAt;

        var transaction = _bookingContext.Database.BeginTransaction();        
        try
        {

            _additionalRepository.Update(Additional);
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
        var entity = _additionalRepository.GetByGuid(guid);
        if(entity == null) return -1;

        var transaction = _bookingContext.Database.BeginTransaction();
        try
        {
            _additionalRepository.Delete(entity);
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
