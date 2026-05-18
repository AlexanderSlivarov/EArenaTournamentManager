using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.DTOs.Auth
{
    public class RegisterUserRequest
    {
        public required string Username { get; set; } 
        public required string Email { get; set; }
        public required string Password { get; set; } 
        public string? AvatarImageUrl { get; set; }
    }
}
