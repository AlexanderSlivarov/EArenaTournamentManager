namespace EArenaTournamentManager.Web.Models.Users
{
    public class UserRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string? AvatarImageUrl { get; set; }
        public string Role { get; set; } = "Member";
    }
}
