using EArenaTournamentManager.Web.Models.Shared;
using EArenaTournamentManager.Web.Models.TournamentParticipants;

namespace EArenaTournamentManager.Web.Services
{
    public class TournamentParticipantService : BaseService
    {
        public TournamentParticipantService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        { }

        public async Task<ServiceResult<PagedData<TournamentParticipantResponse>>?> GetAllAsync(string? token = null)
        {
            return await GetJsonAsync<ServiceResult<PagedData<TournamentParticipantResponse>>>("/api/tournamentparticipants", token);
        }

        public async Task<ServiceResult<PagedData<TournamentParticipantResponse>>?> GetAllAsync(int? tournamentId, string? token = null)
        {
            return await GetAllAsync(tournamentId, null, null, token);
        }

        public async Task<ServiceResult<PagedData<TournamentParticipantResponse>>?> GetAllAsync(int? tournamentId, int? page, int? pageSize, string? token = null)
        {
            var parts = new List<string>();
            if (tournamentId.HasValue && tournamentId.Value > 0) parts.Add($"Filter.TournamentId={tournamentId.Value}");
            if (page.HasValue && page.Value > 0) parts.Add($"Pager.Page={page.Value}");
            if (pageSize.HasValue && pageSize.Value > 0) parts.Add($"Pager.PageSize={pageSize.Value}");

            var uri = parts.Count > 0 ? $"/api/tournamentparticipants?{string.Join("&", parts)}" : "/api/tournamentparticipants";
            return await GetJsonAsync<ServiceResult<PagedData<TournamentParticipantResponse>>>(uri, token);
        }

        public async Task<ServiceResult<TournamentParticipantResponse>?> GetByIdAsync(int id, string? token = null)
        {
            return await GetJsonAsync<ServiceResult<TournamentParticipantResponse>>($"/api/tournamentparticipants/{id}", token);
        }

        public async Task<ServiceResult<TournamentParticipantResponse>?> CreateAsync(TournamentParticipantRequest request, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<TournamentParticipantResponse>>(HttpMethod.Post, "/api/tournamentparticipants", request, token);
        }

        public async Task<ServiceResult<TournamentParticipantResponse>?> UpdateAsync(int id, TournamentParticipantRequest request, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<TournamentParticipantResponse>>(HttpMethod.Put, $"/api/tournamentparticipants/{id}", request, token);
        }

        public async Task<ServiceResult<TournamentParticipantResponse>?> DeleteAsync(int id, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<TournamentParticipantResponse>>(HttpMethod.Delete, $"/api/tournamentparticipants/{id}", null, token);
        }
    }
}
