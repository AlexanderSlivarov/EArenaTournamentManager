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

        public async Task<ServiceResult<PagedData<TeamResponse>>?> GetAllAsync(string? name, int? captainId, string? token = null)
        {
            return await GetAllAsync(name, captainId, null, null, token);
        }

        public async Task<ServiceResult<PagedData<TeamResponse>>?> GetAllAsync(string? name, int? captainId, int? page, int? pageSize, string? token = null)
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(name)) parts.Add($"Filter.Name={Uri.EscapeDataString(name)}");
            if (captainId.HasValue && captainId.Value > 0) parts.Add($"Filter.CaptainId={captainId.Value}");
            if (page.HasValue && page.Value > 0) parts.Add($"Pager.Page={page.Value}");
            if (pageSize.HasValue && pageSize.Value > 0) parts.Add($"Pager.PageSize={pageSize.Value}");

            var uri = parts.Count > 0 ? $"/api/teams?{string.Join("&", parts)}" : "/api/teams";
            return await GetJsonAsync<ServiceResult<PagedData<TeamResponse>>>(uri, token);
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
