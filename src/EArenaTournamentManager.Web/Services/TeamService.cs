using EArenaTournamentManager.Web.Models.Shared;
using EArenaTournamentManager.Web.Models.Teams;

namespace EArenaTournamentManager.Web.Services
{
    public class TeamService : BaseService
    {
        public TeamService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) 
        { }

        public async Task<ServiceResult<PagedData<TeamResponse>>?> GetAllAsync(string? token = null)
        {
            var client = CreateClient(token);
            return await client.GetFromJsonAsync<ServiceResult<PagedData<TeamResponse>>>("/api/teams");
        }

        public async Task<ServiceResult<TeamResponse>?> GetByIdAsync(int id, string? token = null)
        {
            var client = CreateClient(token);
            return await client.GetFromJsonAsync<ServiceResult<TeamResponse>>($"/api/teams/{id}");
        }

        public async Task<ServiceResult<TeamResponse>?> CreateAsync(TeamRequest request, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.PostAsJsonAsync("/api/teams", request);
            return await response.Content.ReadFromJsonAsync<ServiceResult<TeamResponse>>();
        }

        public async Task<ServiceResult<TeamResponse>?> UpdateAsync(int id, TeamRequest request, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.PutAsJsonAsync($"/api/teams/{id}", request);
            return await response.Content.ReadFromJsonAsync<ServiceResult<TeamResponse>>();
        }

        public async Task<ServiceResult<TeamResponse>?> DeleteAsync(int id, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.DeleteAsync($"/api/teams/{id}");
            return await response.Content.ReadFromJsonAsync<ServiceResult<TeamResponse>>();
        }
    }
}
