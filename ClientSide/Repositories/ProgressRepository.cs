using ClientSide.Contract;
using ClientSide.ViewModels.Progress;
using ClientSide.Utilities.Handlers;
using Newtonsoft.Json;

namespace ClientSide.Repositories
{
    public class ProgressRepository : GeneralRepository<ProgressVM>, IProgressRepository
    {
        public ProgressRepository(string request = "Progress/") : base(request)
        {
        }
        public async Task<ResponseHandlers<ProgressVM>> GetProgressById(Guid guid)
        {
            return await Get(guid);
        }

        public async Task<ResponseHandlers<IEnumerable<ProgressVM>>> GetAllProgress()
        {
            return await Get();
        }

        public async Task<ResponseHandlers<ProgressVM>> CreateProgress(ProgressVM progress)
        {
            return await Post(progress);
        }

        public async Task<ResponseHandlers<ProgressVM>> UpdateProgress(ProgressVM progress)
        {
            return await Put(progress);
        }

        /*public async Task<ResponseHandlers<bool>> DeleteProgress(Guid guid)
        {
            return await Delete(guid);
           
        }*/

        /* public async Task<ResponseHandlers<IEnumerable<ProgressVM>>> GetProgress(Guid guid)
         {
             ResponseHandlers<IEnumerable<ProgressVM>> entityVM = null;
             using (var response = await _httpClient.GetAsync(_request + "Manager/" + guid))
             {
                 string apiResponse = await response.Content.ReadAsStringAsync();
                 entityVM = JsonConvert.DeserializeObject<ResponseHandlers<IEnumerable<ProgressVM>>>(apiResponse);
             }
             return entityVM;
         }*/
    }
}
