using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.ResponseDTOs.Users
{
    public class UserResponse
    {
        public int Id { get; set; }

        public required string Username { get; set; }
        public required string Email { get; set; }
        public string? AvatarImageUrl { get; set; }

        public UserRole? Role { get; set; }
        public string RoleName => Role.ToString();
    }
}
