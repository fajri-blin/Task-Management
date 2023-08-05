using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.AccountProgress;

namespace ClientSide.Contract;

public interface IAccountProgressRepository : IGeneralRepository<AccountProgressVM>
{
    Task<ResponseHandlers<IEnumerable<AccountProgressVM>>> GetByProgress(Guid guid);
    Task<ResponseHandlers<IEnumerable<AccountProgressVM>>> GetByAccount(Guid guid);
    Task<ResponseHandlers<AssignStaff>> AddAccountProgress(AssignStaff accountProgressVM);
    Task<ResponseHandlers<Guid>> DeleteAccountProgress(Guid guid);
}
