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
            var client = CreateClient(token);
            return await client.GetFromJsonAsync<ServiceResult<PagedData<TournamentParticipantResponse>>>("/api/tournamentparticipants");
        }

        public async Task<ServiceResult<TournamentParticipantResponse>?> GetByIdAsync(int id, string? token = null)
        {
            var client = CreateClient(token);
            return await client.GetFromJsonAsync<ServiceResult<TournamentParticipantResponse>>($"/api/tournamentparticipants/{id}");
        }

        public async Task<ServiceResult<TournamentParticipantResponse>?> CreateAsync(TournamentParticipantRequest request, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.PostAsJsonAsync("/api/tournamentparticipants", request);
            return await response.Content.ReadFromJsonAsync<ServiceResult<TournamentParticipantResponse>>();
        }

        public async Task<ServiceResult<TournamentParticipantResponse>?> UpdateAsync(int id, TournamentParticipantRequest request, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.PutAsJsonAsync($"/api/tournamentparticipants/{id}", request);
            return await response.Content.ReadFromJsonAsync<ServiceResult<TournamentParticipantResponse>>();
        }

        public async Task<ServiceResult<TournamentParticipantResponse>?> DeleteAsync(int id, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.DeleteAsync($"/api/tournamentparticipants/{id}");
            return await response.Content.ReadFromJsonAsync<ServiceResult<TournamentParticipantResponse>>();
        }
    }
}
