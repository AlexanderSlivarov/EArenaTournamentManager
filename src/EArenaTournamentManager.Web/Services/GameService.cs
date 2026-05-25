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
            var client = CreateClient(token);
            return await client.GetFromJsonAsync<ServiceResult<PagedData<GameResponse>>>(
                "/api/games");
        }

        public async Task<ServiceResult<GameResponse>?> GetByIdAsync(int id, string? token = null)
        {
            var client = CreateClient(token);
            return await client.GetFromJsonAsync<ServiceResult<GameResponse>>(
                $"/api/games/{id}");
        }

        public async Task<ServiceResult<GameResponse>?> CreateAsync(GameRequest request, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.PostAsJsonAsync("/api/games", request);
            return await response.Content.ReadFromJsonAsync<ServiceResult<GameResponse>>();
        }

        public async Task<ServiceResult<GameResponse>?> UpdateAsync(int id, GameRequest request, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.PutAsJsonAsync($"/api/games/{id}", request);
            return await response.Content.ReadFromJsonAsync<ServiceResult<GameResponse>>();
        }

        public async Task<ServiceResult<GameResponse>?> DeleteAsync(int id, string? token = null)
        {
            var client = CreateClient(token);
            var response = await client.DeleteAsync($"/api/games/{id}");
            return await response.Content.ReadFromJsonAsync<ServiceResult<GameResponse>>();
        }
    }
}
