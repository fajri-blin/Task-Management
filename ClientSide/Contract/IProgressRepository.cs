using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Progress;

namespace ClientSide.Contract
{
    public interface IProgressRepository : IGeneralRepository<ProgressVM>
    {
        Task<ResponseHandlers<IEnumerable<ProgressVM>>> GetAllProgress();

        /*Task<ResponseHandlers<IEnumerable<ProgressVM>>> GetAllProgressByAssignmentGuid(Guid assignmentGuid);*/
        Task<ResponseHandlers<CreateProgressVM>> CreateProgress(CreateProgressVM createProgress);
        Task<ResponseHandlers<UpdateProgressVM>> UpdateProgress(UpdateProgressVM updateProgress);
        Task<ResponseHandlers<Guid>> DeepDeleteProgress(Guid guid);
        /*   Task<ResponseHandlers<Guid>> DeleteProgress(Guid guid);*/
    }
}
