using ClientSide.ViewModels.Progress;
using ClientSide.Utilities.Handlers;


namespace ClientSide.Contract
{
    public interface IProgressRepository : IGeneralRepository<ProgressVM>
    {
        Task<ResponseHandlers<IEnumerable<ProgressVM>>> GetAllProgress(Guid guid);
        Task<ResponseHandlers<CreateProgressVM>> CreateProgress(CreateProgressVM createProgress);
        Task<ResponseHandlers<UpdateProgressVM>> UpdateProgress(UpdateProgressVM updateProgress);
        Task<ResponseHandlers<Guid>> DeepDeleteProgress(Guid guid);
        Task<ResponseHandlers<ProgressVM>> GetProgressById(Guid guid);
        Task<ResponseHandlers<ProgressVM>> UpdateProgress(ProgressVM progress);
        
    }
}
