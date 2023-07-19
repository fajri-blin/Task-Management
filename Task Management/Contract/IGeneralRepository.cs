namespace Task_Management.Contract;

public interface IGeneralRepository<TEntity>
{
    IEnumerable<TEntity> GetAll();
    TEntity? GetByGuid(Guid guid);
    TEntity? Create(TEntity entity);
    bool Update(TEntity entity);
    bool Delete(TEntity entity);
    bool IsExits(Guid guid);
}
