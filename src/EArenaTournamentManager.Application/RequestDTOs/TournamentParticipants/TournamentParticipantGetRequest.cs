using EArenaTournamentManager.Application.RequestDTOs.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.TournamentParticipants
{
    public class TournamentParticipantGetRequest : BaseGetRequest
    {
        public TournamentParticipantGetFilterRequest Filter { get; set; } = new TournamentParticipantGetFilterRequest();
    }
}
