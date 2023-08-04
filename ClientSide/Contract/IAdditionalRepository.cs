using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Additional;
using Microsoft.AspNetCore.Mvc;

namespace ClientSide.Contract
{
    public interface IAdditionalRepository : IGeneralRepository<AdditionalVM>
    {
        Task<ResponseHandlers<AdditionalVM>> PostAdditional([FromForm] CreateAdditionalVM createAdditionalVM);
        Task<ResponseHandlers<AdditionalVM>> PutAdditional([FromForm] CreateAdditionalVM createAdditionalVM);
        Task<ResponseHandlers<IEnumerable<AdditionalVM>>> GetAdditional(Guid guid);
    }
}
