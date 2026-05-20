using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.TeamMembers
{
    public class TeamMemberRequest
    {
        public int UserId { get; set; }
        public int TeamId { get; set; }
        
        public string? Role { get; set; }
    }
}
