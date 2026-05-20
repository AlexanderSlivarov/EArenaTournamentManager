using EArenaTournamentManager.Application.RequestDTOs.Teams;
using EArenaTournamentManager.Application.ResponseDTOs.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.ResponseDTOs.Teams
{
    public class TeamGetResponse : BaseGetResponse<TeamResponse>
    {
        public TeamGetFilterRequest Filter { get; set; }
    }
}
