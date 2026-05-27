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

        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? AvatarImageUrl { get; set; }
        public int CreatedBy { get; set; }
        public long CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public long? UpdatedOn { get; set; }
        public bool IsActive { get; set; }

        public UserRole? Role { get; set; }
        public string RoleName => Role.ToString()!;
    }
}
