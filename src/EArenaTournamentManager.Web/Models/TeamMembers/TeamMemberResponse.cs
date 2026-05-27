namespace EArenaTournamentManager.Web.Models.TeamMembers
{
    public class TeamMemberResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TeamId { get; set; }
        public int CreatedBy { get; set; }
        public long CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public long? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public string? Role { get; set; }
        public long JoinedOn { get; set; }
    }
}
