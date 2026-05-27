using EArenaTournamentManager.Web.Models.Games;
using EArenaTournamentManager.Web.Models.Shared;

namespace EArenaTournamentManager.Web.Services
{
    public class GameService : BaseService
    {
        public GameService(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory) { }

        public async Task<ServiceResult<PagedData<GameResponse>>?> GetAllAsync(string? token = null)
        {
            return await GetJsonAsync<ServiceResult<PagedData<GameResponse>>>("/api/games", token);
        }

        public async Task<ServiceResult<GameResponse>?> GetByIdAsync(int id, string? token = null)
        {
            return await GetJsonAsync<ServiceResult<GameResponse>>($"/api/games/{id}", token);
        }

        public async Task<ServiceResult<GameResponse>?> CreateAsync(GameRequest request, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<GameResponse>>(HttpMethod.Post, "/api/games", request, token);
        }

        public async Task<ServiceResult<GameResponse>?> UpdateAsync(int id, GameRequest request, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<GameResponse>>(HttpMethod.Put, $"/api/games/{id}", request, token);
        }

        public async Task<ServiceResult<GameResponse>?> DeleteAsync(int id, string? token = null)
        {
            return await SendJsonAsync<ServiceResult<GameResponse>>(HttpMethod.Delete, $"/api/games/{id}", null, token);
        }
    }
}
