using System.Net.Http.Headers;

namespace EArenaTournamentManager.Web.Services
{
    public abstract class BaseService
    {
        protected readonly IHttpClientFactory _httpClientFactory;

        protected BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        protected HttpClient CreateClient(string? token = null)
        {
            var client = _httpClientFactory.CreateClient("EArenaAPI");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }
    }
}
