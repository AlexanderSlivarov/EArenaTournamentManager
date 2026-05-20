using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.ResponseDTOs.Auth
{
    public class AuthResponse
    {
        public int Id { get; set; }
        public required string Username { get; set; } 
        public required string Email { get; set; } 
        public required string Token { get; set; }
    }
}
