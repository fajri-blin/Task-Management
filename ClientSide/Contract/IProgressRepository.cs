using ClientSide.ViewModels.Progress;
using ClientSide.Utilities.Handlers;


namespace ClientSide.Contract
{
    public interface IProgressRepository : IGeneralRepository<ProgressVM>
    {
        Task<ResponseHandlers<ProgressVM>> GetProgressById(Guid guid);
        Task<ResponseHandlers<IEnumerable<ProgressVM>>> GetAllProgress();
        Task<ResponseHandlers<ProgressVM>> CreateProgress(ProgressVM progress);
        Task<ResponseHandlers<ProgressVM>> UpdateProgress(ProgressVM progress);
     /*   Task<ResponseHandlers<bool>> DeleteProgress(Guid guid);*/
    }
}
