namespace EArenaTournamentManager.Web.Models.Shared
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public List<ApiError>? Errors { get; set; }
    }
}
