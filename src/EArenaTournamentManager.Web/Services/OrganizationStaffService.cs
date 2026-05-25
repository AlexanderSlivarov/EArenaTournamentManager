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
            var client = CreateClient(token);
            return await client.GetFromJsonAsync<ServiceResult<PagedData<OrganizationStaffResponse>>>("/api/organizationstaff");
        }

        public async Task<ServiceResult<OrganizationStaffResponse>?> GetByIdAsync(int id, string? token = null)
        {
            var client = CreateClient(token);
            return await client.GetFromJsonAsync<ServiceResult<OrganizationStaffResponse>>($"/api/organizationstaff/{id}");
        }

        public async Task<ServiceResult<OrganizationStaffResponse>?> CreateAsync(OrganizationStaffRequest request, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.PostAsJsonAsync("/api/organizationstaff", request);
            return await response.Content.ReadFromJsonAsync<ServiceResult<OrganizationStaffResponse>>();
        }

        public async Task<ServiceResult<OrganizationStaffResponse>?> UpdateAsync(int id, OrganizationStaffRequest request, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.PutAsJsonAsync($"/api/organizationstaff/{id}", request);
            return await response.Content.ReadFromJsonAsync<ServiceResult<OrganizationStaffResponse>>();
        }

        public async Task<ServiceResult<OrganizationStaffResponse>?> DeleteAsync(int id, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.DeleteAsync($"/api/organizationstaff/{id}");
            return await response.Content.ReadFromJsonAsync<ServiceResult<OrganizationStaffResponse>>();
        }
    }
}
