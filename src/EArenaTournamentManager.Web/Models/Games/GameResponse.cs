namespace EArenaTournamentManager.Web.Models.Games
{
    public class GameResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string Platform { get; set; } = string.Empty;
    }
}
