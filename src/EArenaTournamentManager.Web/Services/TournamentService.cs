using EArenaTournamentManager.Web.Models.Shared;
using EArenaTournamentManager.Web.Models.Tournaments;

namespace EArenaTournamentManager.Web.Services
{
    public class TournamentService : BaseService
    {
        public TournamentService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) 
        { }

        public async Task<ServiceResult<PagedData<TournamentResponse>>?> GetAllAsync(string? token = null)
        {
            var client = CreateClient(token);
            return await client.GetFromJsonAsync<ServiceResult<PagedData<TournamentResponse>>>(
                "/api/tournaments");
        }

        public async Task<ServiceResult<TournamentResponse>?> GetByIdAsync(int id, string? token = null)
        {
            var client = CreateClient(token);
            return await client.GetFromJsonAsync<ServiceResult<TournamentResponse>>(
                $"/api/tournaments/{id}");
        }

        public async Task<ServiceResult<TournamentResponse>?> CreateAsync(TournamentRequest request, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.PostAsJsonAsync("/api/tournaments", request);
            return await response.Content.ReadFromJsonAsync<ServiceResult<TournamentResponse>>();
        }

        public async Task<ServiceResult<TournamentResponse>?> UpdateAsync(int id, TournamentRequest request, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.PutAsJsonAsync($"/api/tournaments/{id}", request);
            return await response.Content.ReadFromJsonAsync<ServiceResult<TournamentResponse>>();
        }

        public async Task<ServiceResult<TournamentResponse>?> DeleteAsync(int id, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.DeleteAsync($"/api/tournaments/{id}");
            return await response.Content.ReadFromJsonAsync<ServiceResult<TournamentResponse>>();
        }
    }
}
