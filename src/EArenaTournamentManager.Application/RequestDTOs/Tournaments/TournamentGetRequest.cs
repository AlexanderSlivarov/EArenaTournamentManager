using EArenaTournamentManager.Application.RequestDTOs.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.Tournaments
{
    public class TournamentGetRequest : BaseGetRequest
    {
        public TournamentGetFilterRequest Filter { get; set; } = new TournamentGetFilterRequest();
    }
}
