using ClientSide.Contract;
using System.Net.Http.Headers;
using System.Net.Http;

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
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7113/api/")


        };
        //Ini untuk Authorize di API
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _contextAccessor.HttpContext?.Session.GetString("JWToken"));
    }
}
