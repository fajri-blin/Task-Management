using ClientSide.Utilities.Handlers;

namespace ClientSide.Contract;

public interface IGeneralRepository<TEntity>
{
    Task<ResponseHandlers<IEnumerable<TEntity>>> Get();
}
