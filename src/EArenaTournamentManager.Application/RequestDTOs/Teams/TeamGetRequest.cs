using EArenaTournamentManager.Application.RequestDTOs.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.Teams
{
    public class TeamGetRequest : BaseGetRequest
    {
       public required TeamGetFilterRequest Filter { get; set; }
    }
}
