using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ClientSide.Repositories;

public class GeneralRepository<TEntity> : IGeneralRepository<TEntity>
    where TEntity : class
{
    protected readonly string _request;
    protected readonly HttpClient _httpClient;
    protected readonly IHttpContextAccessor _contextAccessor;

    public GeneralRepository(string request)
    {
        _request = request;
        _contextAccessor = new HttpContextAccessor();
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7113/api/")
        };
        //Ini untuk Authorize di API
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _contextAccessor.HttpContext?.Session.GetString("JWToken"));
    }

    public async Task<ResponseHandlers<IEnumerable<TEntity>>> Get()
    {
        ResponseHandlers<IEnumerable<TEntity>> entityVM = null;
        using (var response = await _httpClient.GetAsync(_request))
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<IEnumerable<TEntity>>>(apiResponse);
        }
        return entityVM;
    }

    public async Task<ResponseHandlers<TEntity>> Get(Guid guid)
    {
        ResponseHandlers<TEntity> entityVM = null;

        using (var response = await _httpClient.GetAsync(_request + guid))
        {
            string responseApi = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<TEntity>>(responseApi);
        }
        return entityVM;
    }

    public async Task<ResponseHandlers<TEntity>> Post(TEntity entity)
    {
        ResponseHandlers<TEntity> entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(entity), System.Text.Encoding.UTF8, "application/json");
        using (var response = _httpClient.PostAsync(_request, content).Result)
        {
            string responseApi = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<TEntity>>(responseApi);
        }
        return entityVM;
    }

    public async Task<ResponseHandlers<TEntity>> Put(TEntity entity)
    {
        ResponseHandlers<TEntity> entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(entity), System.Text.Encoding.UTF8, "application/json");
        using (var response = _httpClient.PutAsJsonAsync(_request, entity).Result)
        {
            string responseApi = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<TEntity>>(responseApi);
        }
        return entityVM;
    }

    public async Task<ResponseHandlers<TEntity>> Delete(Guid guid)
    {
        ResponseHandlers<TEntity> entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(guid), System.Text.Encoding.UTF8, "application/json");
        using (var response = _httpClient.DeleteAsync(_request + guid).Result)
        {
            string responseApi = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandlers<TEntity>>(responseApi);
        }
        return entityVM;
    
    }
}
