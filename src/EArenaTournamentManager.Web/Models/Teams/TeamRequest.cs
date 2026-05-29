namespace EArenaTournamentManager.Web.Models.Teams
{
    public class TeamRequest
    {
        public int CaptainId { get; set; }
        public string CaptainUsername { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? LogoImageUrl { get; set; }
    }
}
