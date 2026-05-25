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
            var client = CreateClient(token);
            return await client.GetFromJsonAsync<ServiceResult<PagedData<OrganizationResponse>>>("/api/organizations");
        }

        public async Task<ServiceResult<OrganizationResponse>?> GetByIdAsync(int id, string? token = null)
        {
            var client = CreateClient(token);
            return await client.GetFromJsonAsync<ServiceResult<OrganizationResponse>>($"/api/organizations/{id}");
        }

        public async Task<ServiceResult<OrganizationResponse>?> CreateAsync(OrganizationRequest request, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.PostAsJsonAsync("/api/organizations", request);
            return await response.Content.ReadFromJsonAsync<ServiceResult<OrganizationResponse>>();
        }

        public async Task<ServiceResult<OrganizationResponse>?> UpdateAsync(int id, OrganizationRequest request, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.PutAsJsonAsync($"/api/organizations/{id}", request);
            return await response.Content.ReadFromJsonAsync<ServiceResult<OrganizationResponse>>();
        }

        public async Task<ServiceResult<OrganizationResponse>?> DeleteAsync(int id, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.DeleteAsync($"/api/organizations/{id}");
            return await response.Content.ReadFromJsonAsync<ServiceResult<OrganizationResponse>>();
        }
    }
}
