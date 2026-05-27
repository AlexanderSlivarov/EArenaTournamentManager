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
            return await GetJsonAsync<ServiceResult<PagedData<TeamResponse>>>("/api/teams", token);
        }

        public async Task<ServiceResult<TeamResponse>?> GetByIdAsync(int id, string? token = null)
        {
            return await GetJsonAsync<ServiceResult<TeamResponse>>($"/api/teams/{id}", token);
        }

        public async Task<ServiceResult<TeamResponse>?> CreateAsync(TeamRequest request, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<TeamResponse>>(HttpMethod.Post, "/api/teams", request, token);
        }

        public async Task<ServiceResult<TeamResponse>?> UpdateAsync(int id, TeamRequest request, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<TeamResponse>>(HttpMethod.Put, $"/api/teams/{id}", request, token);
        }

        public async Task<ServiceResult<TeamResponse>?> DeleteAsync(int id, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<TeamResponse>>(HttpMethod.Delete, $"/api/teams/{id}", null, token);
        }
    }
}
