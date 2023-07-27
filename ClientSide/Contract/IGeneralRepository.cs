using ClientSide.Utilities.Handlers;

namespace ClientSide.Contract;

public interface IGeneralRepository<TEntity>
{
    Task<ResponseHandlers<IEnumerable<TEntity>>> Get();
    Task<ResponseHandlers<TEntity>> Get(Guid guid);
    Task<ResponseHandlers<TEntity>> Post(TEntity entity);
    Task<ResponseHandlers<TEntity>> Put(TEntity entity);
    Task<ResponseHandlers<TEntity>> Delete(Guid guid);
}
