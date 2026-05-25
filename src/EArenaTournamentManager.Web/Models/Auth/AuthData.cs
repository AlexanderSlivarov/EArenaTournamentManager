using EArenaTournamentManager.Web.Models.Shared;

namespace EArenaTournamentManager.Web.Models.Auth
{    
    public class AuthData
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string? AvatarImageUrl { get; set; }
    }   
}
