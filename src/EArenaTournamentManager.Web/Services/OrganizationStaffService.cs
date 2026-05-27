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
