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
            var client = CreateClient(token);
            return await client.GetFromJsonAsync<ServiceResult<PagedData<TeamMemberResponse>>>("/api/teammembers");
        }

        public async Task<ServiceResult<TeamMemberResponse>?> GetByIdAsync(int id, string? token = null)
        {
            var client = CreateClient(token);
            return await client.GetFromJsonAsync<ServiceResult<TeamMemberResponse>>($"/api/teammembers/{id}");
        }

        public async Task<ServiceResult<TeamMemberResponse>?> CreateAsync(TeamMemberRequest request, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.PostAsJsonAsync("/api/teammembers", request);
            return await response.Content.ReadFromJsonAsync<ServiceResult<TeamMemberResponse>>();
        }

        public async Task<ServiceResult<TeamMemberResponse>?> UpdateAsync(int id, TeamMemberRequest request, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.PutAsJsonAsync($"/api/teammembers/{id}", request);
            return await response.Content.ReadFromJsonAsync<ServiceResult<TeamMemberResponse>>();
        }

        public async Task<ServiceResult<TeamMemberResponse>?> DeleteAsync(int id, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.DeleteAsync($"/api/teammembers/{id}");
            return await response.Content.ReadFromJsonAsync<ServiceResult<TeamMemberResponse>>();
        }
    }
}
