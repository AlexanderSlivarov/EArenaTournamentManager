using EArenaTournamentManager.Web.Models.Auth;
using EArenaTournamentManager.Web.Models.Shared;

namespace EArenaTournamentManager.Web.Services
{
    public class AuthService : BaseService
    {
        public AuthService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        { }

        public async Task<ServiceResult<AuthData>?> LoginAsync(LoginRequest request)
        {
            var client = CreateClient();
            var response = await client.PostAsJsonAsync("/api/auth/login", request);
            return await response.Content.ReadFromJsonAsync<ServiceResult<AuthData>>();
        }

        public async Task<ServiceResult<AuthData>?> RegisterAsync(RegisterRequest request)
        {
            var client = CreateClient();
            var response = await client.PostAsJsonAsync("api/auth/register", request);
            return await response.Content.ReadFromJsonAsync<ServiceResult<AuthData>>();
        }
    }
}
