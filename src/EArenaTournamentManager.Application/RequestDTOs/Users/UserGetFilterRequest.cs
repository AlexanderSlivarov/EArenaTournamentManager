using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.Users
{
    public class UserGetFilterRequest
    {   
        public string? Username { get; set; }
        public string? Email { get; set; }

        public UserRole? Role { get; set; }
    }
}
