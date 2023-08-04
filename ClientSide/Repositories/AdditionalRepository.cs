using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Additional;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ClientSide.Repositories
{
    public class AdditionalRepository : GeneralRepository<AdditionalVM>, IAdditionalRepository
    {

        public AdditionalRepository(string request = "Additional/") : base(request)
        {

        }

        public async Task<ResponseHandlers<IEnumerable<AdditionalVM>>> PostAdditional([FromForm] CreateAdditionalVM createAdditionalVM)
        {
            ResponseHandlers<IEnumerable<AdditionalVM>> entityVM = null;

            using (var formData = new MultipartFormDataContent())
            {
                // Tambahkan foto profile ke form data
                if (createAdditionalVM.FileData != null && createAdditionalVM.FileData.Count > 0)
                {
                    for (int i = 0; i < createAdditionalVM.FileData.Count; i++)
                    {
                        var fileContent = new StreamContent(createAdditionalVM.FileData[i].OpenReadStream())
                        {
                            Headers = { ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "FileName", FileName = createAdditionalVM.FileData[i].FileName } }
                        };
                        formData.Add(fileContent, "FileName", createAdditionalVM.FileData[i].FileName);
                    }
                }

                // Tambahkan data lainnya ke form data
                if (createAdditionalVM.ProgressGuid != null)
                {
                    formData.Add(new StringContent(createAdditionalVM.ProgressGuid.ToString()), "ProgressGuid");
                }

                // Kirim request ke API
                using (var response = await _httpClient.PostAsync(_request, formData))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    entityVM = JsonConvert.DeserializeObject<ResponseHandlers<IEnumerable<AdditionalVM>>>(apiResponse);
                }
            }

            return entityVM;
        }

        public async Task<ResponseHandlers<AdditionalVM>> PutAdditional([FromForm] CreateAdditionalVM createAdditionalVM)
        {
            ResponseHandlers<AdditionalVM> entityVM = null;

            using (var formData = new MultipartFormDataContent())
            {
                // Tambahkan foto profile ke form data
                if (createAdditionalVM.FileData != null && createAdditionalVM.FileData.Count > 0)
                {
                    for (int i = 0; i < createAdditionalVM.FileData.Count; i++)
                    {
                        var fileContent = new StreamContent(createAdditionalVM.FileData[i].OpenReadStream())
                        {
                            Headers = { ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "FileName", FileName = createAdditionalVM.FileData[i].FileName } }
                        };
                        formData.Add(fileContent, "FileName", createAdditionalVM.FileData[i].FileName);
                    }
                }

                // Tambahkan data lainnya ke form data
                if (createAdditionalVM.ProgressGuid != null)
                {
                    formData.Add(new StringContent(createAdditionalVM.ProgressGuid.ToString()), "ProgressGuid");
                }

                // Kirim request ke API
                using (var response = await _httpClient.PutAsync(_request, formData))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    entityVM = JsonConvert.DeserializeObject<ResponseHandlers<AdditionalVM>>(apiResponse);
                }
            }

            return entityVM;
        }

        public async Task<ResponseHandlers<IEnumerable<AdditionalVM>>> GetAdditional(Guid guid)
        {
            ResponseHandlers<IEnumerable<AdditionalVM>> entityVM = null;

            using (var response = await _httpClient.GetAsync(_request + "GetByProgressKey/" + guid))
            {
                string responseApi = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandlers<IEnumerable<AdditionalVM>>>(responseApi);
            }
            return entityVM;
        }
    }
}
