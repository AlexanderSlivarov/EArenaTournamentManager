namespace EArenaTournamentManager.Web.Models.TeamMembers
{
    public class TeamMemberRequest
    {
        public int UserId { get; set; }
        public int TeamId { get; set; }
        public string? Role { get; set; }
    }
}
