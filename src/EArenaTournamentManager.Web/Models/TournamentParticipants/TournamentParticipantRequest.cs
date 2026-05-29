namespace EArenaTournamentManager.Web.Models.TournamentParticipants
{
    public class TournamentParticipantRequest
    {
        public int TournamentId { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
    }
}
