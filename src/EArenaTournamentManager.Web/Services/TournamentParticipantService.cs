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
