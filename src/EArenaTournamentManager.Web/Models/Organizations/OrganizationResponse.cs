namespace EArenaTournamentManager.Web.Models.Organizations
{
    public class OrganizationResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? LogoImageUrl { get; set; }
        public string? HeaderImageUrl { get; set; }
        public string Type { get; set; } = string.Empty;
        public int CreatedBy { get; set; }
        public long CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public long? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
