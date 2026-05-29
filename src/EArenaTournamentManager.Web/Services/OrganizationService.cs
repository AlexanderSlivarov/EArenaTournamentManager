using EArenaTournamentManager.Web.Models.Organizations;
using EArenaTournamentManager.Web.Models.Shared;

namespace EArenaTournamentManager.Web.Services
{
    public class OrganizationService : BaseService
    {
        public OrganizationService(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory) { }

        public async Task<ServiceResult<PagedData<OrganizationResponse>>?> GetAllAsync(string? token = null)
        {
            return await GetJsonAsync<ServiceResult<PagedData<OrganizationResponse>>>("/api/organizations", token);
        }

        public async Task<ServiceResult<PagedData<OrganizationResponse>>?> GetAllAsync(string? name, string? type, string? token = null)
        {
            return await GetAllAsync(name, type, null, null, token);
        }

        public async Task<ServiceResult<PagedData<OrganizationResponse>>?> GetAllAsync(string? name, string? type, int? page, int? pageSize, string? token = null)
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(name)) parts.Add($"Filter.Name={Uri.EscapeDataString(name)}");
            if (!string.IsNullOrWhiteSpace(type)) parts.Add($"Filter.Type={Uri.EscapeDataString(type)}");
            if (page.HasValue && page.Value > 0) parts.Add($"Pager.Page={page.Value}");
            if (pageSize.HasValue && pageSize.Value > 0) parts.Add($"Pager.PageSize={pageSize.Value}");

            var uri = parts.Count > 0 ? $"/api/organizations?{string.Join("&", parts)}" : "/api/organizations";
            return await GetJsonAsync<ServiceResult<PagedData<OrganizationResponse>>>(uri, token);
        }

        public async Task<ServiceResult<OrganizationResponse>?> GetByIdAsync(int id, string? token = null)
        {
            return await GetJsonAsync<ServiceResult<OrganizationResponse>>($"/api/organizations/{id}", token);
        }

        public async Task<ServiceResult<OrganizationResponse>?> CreateAsync(OrganizationRequest request, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<OrganizationResponse>>(HttpMethod.Post, "/api/organizations", request, token);
        }

        public async Task<ServiceResult<OrganizationResponse>?> UpdateAsync(int id, OrganizationRequest request, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<OrganizationResponse>>(HttpMethod.Put, $"/api/organizations/{id}", request, token);
        }

        public async Task<ServiceResult<OrganizationResponse>?> DeleteAsync(int id, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<OrganizationResponse>>(HttpMethod.Delete, $"/api/organizations/{id}", null, token);
        }
    }
}
