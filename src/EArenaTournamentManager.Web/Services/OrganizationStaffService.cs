using EArenaTournamentManager.Web.Models.OrganizationStaff;
using EArenaTournamentManager.Web.Models.Shared;

namespace EArenaTournamentManager.Web.Services
{
    public class OrganizationStaffService : BaseService
    {
        public OrganizationStaffService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) 
        { }

        public async Task<ServiceResult<PagedData<OrganizationStaffResponse>>?> GetAllAsync(string? token = null)
        {
            return await GetJsonAsync<ServiceResult<PagedData<OrganizationStaffResponse>>>("/api/organizationstaff", token);
        }

        public async Task<ServiceResult<PagedData<OrganizationStaffResponse>>?> GetAllAsync(int? organizationId, string? token = null)
        {
            return await GetAllAsync(organizationId, null, null, token);
        }

        public async Task<ServiceResult<PagedData<OrganizationStaffResponse>>?> GetAllAsync(int? organizationId, int? page, int? pageSize, string? token = null)
        {
            var parts = new List<string>();
            if (organizationId.HasValue && organizationId.Value > 0) parts.Add($"Filter.OrganizationId={organizationId.Value}");
            if (page.HasValue && page.Value > 0) parts.Add($"Pager.Page={page.Value}");
            if (pageSize.HasValue && pageSize.Value > 0) parts.Add($"Pager.PageSize={pageSize.Value}");

            var uri = parts.Count > 0 ? $"/api/organizationstaff?{string.Join("&", parts)}" : "/api/organizationstaff";
            return await GetJsonAsync<ServiceResult<PagedData<OrganizationStaffResponse>>>(uri, token);
        }

        public async Task<ServiceResult<OrganizationStaffResponse>?> GetByIdAsync(int id, string? token = null)
        {
            return await GetJsonAsync<ServiceResult<OrganizationStaffResponse>>($"/api/organizationstaff/{id}", token);
        }

        public async Task<ServiceResult<OrganizationStaffResponse>?> CreateAsync(OrganizationStaffRequest request, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<OrganizationStaffResponse>>(HttpMethod.Post, "/api/organizationstaff", request, token);
        }

        public async Task<ServiceResult<OrganizationStaffResponse>?> UpdateAsync(int id, OrganizationStaffRequest request, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<OrganizationStaffResponse>>(HttpMethod.Put, $"/api/organizationstaff/{id}", request, token);
        }

        public async Task<ServiceResult<OrganizationStaffResponse>?> DeleteAsync(int id, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<OrganizationStaffResponse>>(HttpMethod.Delete, $"/api/organizationstaff/{id}", null, token);
        }
    }
}
