using EArenaTournamentManager.Web.Models.Shared;
using EArenaTournamentManager.Web.Models.Users;

namespace EArenaTournamentManager.Web.Services
{
    public class UserService : BaseService
    {
        public UserService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) 
        { }

        public async Task<ServiceResult<PagedData<UserResponse>>?> GetAllAsync(string? token = null)
        {
            var client = CreateClient(token);
            return await client.GetFromJsonAsync<ServiceResult<PagedData<UserResponse>>>(
                "/api/users");
        }

        public async Task<ServiceResult<UserResponse>?> GetByIdAsync(int id, string? token = null)
        {
            var client = CreateClient(token);
            return await client.GetFromJsonAsync<ServiceResult<UserResponse>>(
                $"/api/users/{id}");
        }

        public async Task<ServiceResult<UserResponse>?> CreateAsync(UserRequest request, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.PostAsJsonAsync("/api/users", request);
            return await response.Content.ReadFromJsonAsync<ServiceResult<UserResponse>>();
        }

        public async Task<ServiceResult<UserResponse>?> UpdateAsync(int id, UserRequest request, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.PutAsJsonAsync($"/api/users/{id}", request);
            return await response.Content.ReadFromJsonAsync<ServiceResult<UserResponse>>();
        }

        public async Task<ServiceResult<UserResponse>?> DeleteAsync(int id, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.DeleteAsync($"/api/users/{id}");
            return await response.Content.ReadFromJsonAsync<ServiceResult<UserResponse>>();
        }
    }
}
