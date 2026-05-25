namespace EArenaTournamentManager.Web.Models.Organizations
{
    public class OrganizationRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? LogoImageUrl { get; set; }
        public string? HeaderImageUrl { get; set; }
        public string Type { get; set; } = "Personal";
    }
}
