namespace EArenaTournamentManager.Web.Models.OrganizationStaff
{
    public class OrganizationStaffRequest
    {
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; } = "Staff";
    }
}
