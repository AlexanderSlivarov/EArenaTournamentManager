using System.Net.Http.Headers;
using System.Net;
using System.Net.Http.Json;
using EArenaTournamentManager.Web.Exceptions;

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

        protected async Task<T?> GetJsonAsync<T>(string requestUri, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.GetAsync(requestUri);
            EnsureAuthorized(response);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        protected async Task<T?> SendJsonAsync<T>(HttpMethod method, string requestUri, object? body = null, string? token = null)
        {
            var client = CreateClient(token);
            using var request = new HttpRequestMessage(method, requestUri);

            if (body is not null)
            {
                request.Content = JsonContent.Create(body);
            }

            var response = await client.SendAsync(request);
            EnsureAuthorized(response);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        private static void EnsureAuthorized(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new SessionExpiredException();
            }
        }
    }
}
