using EArenaTournamentManager.Application.RequestDTOs.Tournaments;
using EArenaTournamentManager.Application.ResponseDTOs.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.ResponseDTOs.Tournaments
{
    public class TournamentGetResponse : BaseGetResponse<TournamentResponse>
    {
        public TournamentGetFilterRequest Filter { get; set; }
    }
}
