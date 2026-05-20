using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.TeamMembers
{
    public class TeamMemberGetFilterRequest
    {
        public int? UserId { get; set; }
        public int? TeamId { get; set; }
    }
}
