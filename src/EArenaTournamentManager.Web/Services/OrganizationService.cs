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
