namespace EArenaTournamentManager.Web.Models.Shared
{
    public class ApiError
    {
        public string Key { get; set; } = string.Empty;
        public List<string> Messages { get; set; } = new();
    }
}
