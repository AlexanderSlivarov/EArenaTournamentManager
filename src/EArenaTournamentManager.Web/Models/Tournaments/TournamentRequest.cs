namespace EArenaTournamentManager.Web.Models.Tournaments
{
    public class TournamentRequest
    {
        public int GameId { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Format { get; set; }
        public string? Map { get; set; }
        public string? Region { get; set; }
        public string? Rules { get; set; }
        public string? Prizes { get; set; }
        public long DateTime { get; set; }
        public string Status { get; set; } = "Open";
    }
}