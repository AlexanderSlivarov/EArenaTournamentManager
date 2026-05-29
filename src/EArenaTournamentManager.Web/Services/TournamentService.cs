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

        public async Task<ServiceResult<PagedData<TournamentResponse>>?> GetAllAsync(int? gameId, int? organizationId, string? name, string? status, string? region, long? dateFrom, long? dateTo, string? token = null)
        {
            return await GetAllAsync(gameId, organizationId, name, status, region, dateFrom, dateTo, null, null, token);
        }

        public async Task<ServiceResult<PagedData<TournamentResponse>>?> GetAllAsync(int? gameId, int? organizationId, string? name, string? status, string? region, long? dateFrom, long? dateTo, int? page, int? pageSize, string? token = null)
        {
            var parts = new List<string>();
            if (gameId.HasValue && gameId.Value > 0) parts.Add($"Filter.GameId={gameId.Value}");
            if (organizationId.HasValue && organizationId.Value > 0) parts.Add($"Filter.OrganizationId={organizationId.Value}");
            if (!string.IsNullOrWhiteSpace(name)) parts.Add($"Filter.Name={Uri.EscapeDataString(name)}");
            if (!string.IsNullOrWhiteSpace(status)) parts.Add($"Filter.Status={Uri.EscapeDataString(status)}");
            if (!string.IsNullOrWhiteSpace(region)) parts.Add($"Filter.Region={Uri.EscapeDataString(region)}");
            if (dateFrom.HasValue) parts.Add($"Filter.DateFrom={dateFrom.Value}");
            if (dateTo.HasValue) parts.Add($"Filter.DateTo={dateTo.Value}");
            if (page.HasValue && page.Value > 0) parts.Add($"Pager.Page={page.Value}");
            if (pageSize.HasValue && pageSize.Value > 0) parts.Add($"Pager.PageSize={pageSize.Value}");

            var uri = parts.Count > 0 ? $"/api/tournaments?{string.Join("&", parts)}" : "/api/tournaments";
            return await GetJsonAsync<ServiceResult<PagedData<TournamentResponse>>>(uri, token);
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
