using EArenaTournamentManager.Web.Models.Shared;
using EArenaTournamentManager.Web.Models.TeamMembers;

namespace EArenaTournamentManager.Web.Services
{
    public class TeamMemberService : BaseService
    {
        public TeamMemberService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) 
        { }

        public async Task<ServiceResult<PagedData<TeamMemberResponse>>?> GetAllAsync(string? token = null)
        {
            return await GetJsonAsync<ServiceResult<PagedData<TeamMemberResponse>>>("/api/teammembers", token);
        }

        public async Task<ServiceResult<PagedData<TeamMemberResponse>>?> GetAllAsync(int? teamId, string? token = null)
        {
            return await GetAllAsync(teamId, null, null, token);
        }

        public async Task<ServiceResult<PagedData<TeamMemberResponse>>?> GetAllAsync(int? teamId, int? page, int? pageSize, string? token = null)
        {
            var parts = new List<string>();
            if (teamId.HasValue && teamId.Value > 0) parts.Add($"Filter.TeamId={teamId.Value}");
            if (page.HasValue && page.Value > 0) parts.Add($"Pager.Page={page.Value}");
            if (pageSize.HasValue && pageSize.Value > 0) parts.Add($"Pager.PageSize={pageSize.Value}");

            var uri = parts.Count > 0 ? $"/api/teammembers?{string.Join("&", parts)}" : "/api/teammembers";
            return await GetJsonAsync<ServiceResult<PagedData<TeamMemberResponse>>>(uri, token);
        }

        public async Task<ServiceResult<TeamMemberResponse>?> GetByIdAsync(int id, string? token = null)
        {
            return await GetJsonAsync<ServiceResult<TeamMemberResponse>>($"/api/teammembers/{id}", token);
        }

        public async Task<ServiceResult<TeamMemberResponse>?> CreateAsync(TeamMemberRequest request, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<TeamMemberResponse>>(HttpMethod.Post, "/api/teammembers", request, token);
        }

        public async Task<ServiceResult<TeamMemberResponse>?> UpdateAsync(int id, TeamMemberRequest request, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<TeamMemberResponse>>(HttpMethod.Put, $"/api/teammembers/{id}", request, token);
        }

        public async Task<ServiceResult<TeamMemberResponse>?> DeleteAsync(int id, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<TeamMemberResponse>>(HttpMethod.Delete, $"/api/teammembers/{id}", null, token);
        }
    }
}
