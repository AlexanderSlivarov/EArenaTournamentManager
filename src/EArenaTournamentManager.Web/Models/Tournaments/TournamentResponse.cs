namespace EArenaTournamentManager.Web.Models.Tournaments
{
    public class TournamentResponse
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FullDescription { get; set; } = string.Empty;
        public string? LogoImageUrl { get; set; }
        public string? Format { get; set; }
        public string? Map { get; set; }
        public string? Region { get; set; }
        public string? Rules { get; set; }
        public string? Prizes { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public long DateTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public int CreatedBy { get; set; }
        public long CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public long? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}