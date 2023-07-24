using Task_Management.Contract;
using Task_Management.Data;

namespace Task_Management.Repository;

public class GeneralRepository<TEntity> : IGeneralRepository<TEntity>
    where TEntity : class
{
    protected readonly BookingDbContext _bookingDbContext;

    public GeneralRepository(BookingDbContext bookingDbContext)
    {
        _bookingDbContext = bookingDbContext;
    }

    public IEnumerable<TEntity> GetAll()
    {
        return _bookingDbContext.Set<TEntity>().ToList();
    }

    public TEntity? GetByGuid(Guid guid)
    {
        var entity =  _bookingDbContext.Set<TEntity>().Find(guid);
        _bookingDbContext.ChangeTracker.Clear();
        return entity;
    }

    public TEntity? Create(TEntity entity)
    {
        try
        {
            _bookingDbContext.Set<TEntity>().Add(entity);
            _bookingDbContext.SaveChanges();
            return entity;
        }
        catch
        {
            return null;
        }
    }

    public bool Update(TEntity entity)
    {
        try
        {
            _bookingDbContext.Set<TEntity>().Update(entity);
            _bookingDbContext.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
    public bool Delete(TEntity entity)
    {
        try
        {
            _bookingDbContext.Set<TEntity>().Remove(entity);
            _bookingDbContext.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool IsExits(Guid guid)
    {
        return GetByGuid(guid) != null;
    }

}
