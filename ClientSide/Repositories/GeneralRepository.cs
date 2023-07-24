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
}
