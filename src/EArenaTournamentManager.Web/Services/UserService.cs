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
            return await GetJsonAsync<ServiceResult<PagedData<UserResponse>>>("/api/users", token);
        }

        public async Task<ServiceResult<UserResponse>?> GetByIdAsync(int id, string? token = null)
        {
            return await GetJsonAsync<ServiceResult<UserResponse>>($"/api/users/{id}", token);
        } 

        public async Task<ServiceResult<UserResponse>?> CreateAsync(UserRequest request, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<UserResponse>>(HttpMethod.Post, "/api/users", request, token);
        }

        public async Task<ServiceResult<UserResponse>?> UpdateAsync(int id, UserRequest request, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<UserResponse>>(HttpMethod.Put, $"/api/users/{id}", request, token);
        }

        public async Task<ServiceResult<UserResponse>?> DeleteAsync(int id, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<UserResponse>>(HttpMethod.Delete, $"/api/users/{id}", null, token);
        }
    }
}
