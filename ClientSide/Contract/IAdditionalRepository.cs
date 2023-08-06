using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Additional;
using Microsoft.AspNetCore.Mvc;

namespace ClientSide.Contract
{
    public interface IAdditionalRepository : IGeneralRepository<AdditionalVM>
    {
        Task<ResponseHandlers<IEnumerable<AdditionalVM>>> PostAdditional([FromForm] CreateAdditionalVM createAdditionalVM);
        Task<ResponseHandlers<IEnumerable<AdditionalVM>>> GetAdditional(Guid guid);
        Task<ResponseHandlers<AdditionalVM>> DeleteAdditional(Guid guid);
        Task<FileResult> DownloadFile(Guid guid);
    }
}
