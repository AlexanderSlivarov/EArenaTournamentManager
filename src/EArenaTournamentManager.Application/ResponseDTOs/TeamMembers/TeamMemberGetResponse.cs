using EArenaTournamentManager.Application.RequestDTOs.TeamMembers;
using EArenaTournamentManager.Application.ResponseDTOs.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.ResponseDTOs.TeamMembers
{
    public class TeamMemberGetResponse : BaseGetResponse<TeamMemberResponse>
    {
        public TeamMemberGetFilterRequest? Filter { get; set; }
    }
}
