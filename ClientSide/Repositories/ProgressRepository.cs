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

        /*public async Task<ResponseHandlers<Guid>> DeleteProgress(Guid guid)
        {
            return await Delete(guid);
           
        }*/
        public async Task<ResponseHandlers<Guid>> DeepDeleteProgress(Guid guid)
        {
            ResponseHandlers<Guid> entityVM = null;

            using (var response = await _httpClient.DeleteAsync(_request + "DeepDelete/" + guid))
            {
                string responseApi = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<ResponseHandlers<object>>(responseApi);

                // Check if the data field is null
                if (responseObject.Data == null)
                {
                    entityVM = new ResponseHandlers<Guid>
                    {
                        Code = responseObject.Code,
                        Message = responseObject.Message
                    };
                }
                else
                {
                    // If the data field is not null, deserialize it to Guid
                    entityVM = new ResponseHandlers<Guid>
                    {
                        Code = responseObject.Code,
                        Message = responseObject.Message,
                        Data = Guid.Parse(responseObject.Data.ToString())
                    };
                }
            }

            return entityVM;
        }
    }
}
