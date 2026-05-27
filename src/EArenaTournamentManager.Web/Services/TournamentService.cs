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
            return await GetJsonAsync<ServiceResult<PagedData<TournamentResponse>>>("/api/tournaments", token);
        }

        public async Task<ServiceResult<TournamentResponse>?> GetByIdAsync(int id, string? token = null)
        {
            return await GetJsonAsync<ServiceResult<TournamentResponse>>($"/api/tournaments/{id}", token);
        }

        public async Task<ServiceResult<TournamentResponse>?> CreateAsync(TournamentRequest request, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<TournamentResponse>>(HttpMethod.Post, "/api/tournaments", request, token);
        }

        public async Task<ServiceResult<TournamentResponse>?> UpdateAsync(int id, TournamentRequest request, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<TournamentResponse>>(HttpMethod.Put, $"/api/tournaments/{id}", request, token);
        }

        public async Task<ServiceResult<TournamentResponse>?> DeleteAsync(int id, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<TournamentResponse>>(HttpMethod.Delete, $"/api/tournaments/{id}", null, token);
        }
    }
}
