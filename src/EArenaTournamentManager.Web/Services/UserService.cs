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

        public async Task<ServiceResult<PagedData<UserResponse>>?> GetAllAsync(string? username, string? email, string? role, string? token = null)
        {
            return await GetAllAsync(username, email, role, null, null, token);
        }

        public async Task<ServiceResult<PagedData<UserResponse>>?> GetAllAsync(string? username, string? email, string? role, int? page, int? pageSize, string? token = null)
        {
            var queryParts = new List<string>();

            if (!string.IsNullOrWhiteSpace(username))
            {
                queryParts.Add($"Filter.Username={Uri.EscapeDataString(username)}");
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                queryParts.Add($"Filter.Email={Uri.EscapeDataString(email)}");
            }

            if (!string.IsNullOrWhiteSpace(role))
            {
                queryParts.Add($"Filter.Role={Uri.EscapeDataString(role)}");
            }

            if (page.HasValue && page.Value > 0)
            {
                queryParts.Add($"Pager.Page={page.Value}");
            }

            if (pageSize.HasValue && pageSize.Value > 0)
            {
                queryParts.Add($"Pager.PageSize={pageSize.Value}");
            }

            var requestUri = queryParts.Count > 0
                ? $"/api/users?{string.Join("&", queryParts)}"
                : "/api/users";

            return await GetJsonAsync<ServiceResult<PagedData<UserResponse>>>(requestUri, token);
        }

        public async Task<ServiceResult<UserResponse>?> GetByIdAsync(int id, string? token = null)
        {
            return await GetJsonAsync<ServiceResult<UserResponse>>($"/api/users/{id}", token);
        }

        public async Task<UserResponse?> GetByUsernameAsync(string username, string? token = null)
        {
            var result = await GetAllAsync(username, null, null, token);

            return result?.Data?.Items
                .FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
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
