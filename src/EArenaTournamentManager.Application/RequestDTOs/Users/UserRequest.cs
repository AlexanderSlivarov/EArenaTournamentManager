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
        public required string Username { get; set; }
        public string? Password { get; set; }
        public required string Email { get; set; }
        public string? AvatarImageUrl { get; set; }

        public UserRole Role { get; set; }
    }
}
