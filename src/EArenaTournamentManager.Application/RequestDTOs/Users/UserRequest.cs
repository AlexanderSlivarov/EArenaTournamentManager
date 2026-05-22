using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.Users
{
    public class UserRequest
    {
        public string Username { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? AvatarImageUrl { get; set; }

        public UserRole Role { get; set; } = UserRole.Member;
    }
}
