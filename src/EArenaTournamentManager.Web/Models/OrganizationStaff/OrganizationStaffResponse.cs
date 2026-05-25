namespace EArenaTournamentManager.Web.Models.OrganizationStaff
{
    public class OrganizationStaffResponse
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; } = string.Empty;
        public long JoinedOn { get; set; }
    }
}
