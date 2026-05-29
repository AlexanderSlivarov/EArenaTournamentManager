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

        public async Task<ServiceResult<PagedData<GameResponse>>?> GetAllAsync(string? name, string? platform, string? token = null)
        {
            return await GetAllAsync(name, platform, null, null, token);
        }

        public async Task<ServiceResult<PagedData<GameResponse>>?> GetAllAsync(string? name, string? platform, int? page, int? pageSize, string? token = null)
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(name)) parts.Add($"Filter.Name={Uri.EscapeDataString(name)}");
            if (!string.IsNullOrWhiteSpace(platform)) parts.Add($"Filter.Platform={Uri.EscapeDataString(platform)}");
            if (page.HasValue && page.Value > 0) parts.Add($"Pager.Page={page.Value}");
            if (pageSize.HasValue && pageSize.Value > 0) parts.Add($"Pager.PageSize={pageSize.Value}");

            var uri = parts.Count > 0 ? $"/api/games?{string.Join("&", parts)}" : "/api/games";
            return await GetJsonAsync<ServiceResult<PagedData<GameResponse>>>(uri, token);
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
