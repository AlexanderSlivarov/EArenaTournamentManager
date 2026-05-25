namespace EArenaTournamentManager.Web.Models.TournamentParticipants
{
    public class TournamentParticipantResponse
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int TeamId { get; set; }
        public long JoinedOn { get; set; }
    }
}
